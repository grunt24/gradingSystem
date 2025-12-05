using BackendApi.Context;
using BackendApi.Core.General;
using BackendApi.Core.Models;
using BackendApi.Core.Models.Dto;
using BackendApi.IRepositories;
using BackendApi.Repositories;
using BackendApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentSubjectsController : ControllerBase
    {
        private readonly IStudentSubjectService _studentSubjectService;
        private readonly IAuthRepository _authRepository;
        private readonly AppDbContext _context;

        public StudentSubjectsController(IStudentSubjectService studentSubjectService, IAuthRepository authRepository, AppDbContext context)
        {
            _studentSubjectService = studentSubjectService;
            _authRepository = authRepository;
            _context = context;
        }

        // GET: api/StudentSubjects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentSubject>>> GetAllStudentSubjects()
        {
            var studentSubjects = await _studentSubjectService.GetAllStudentSubjects();
            return Ok(studentSubjects);
        }

        // GET: api/StudentSubjects/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentSubject>> GetStudentSubjectById(int id)
        {
            var studentSubject = await _studentSubjectService.GetStudentSubjectById(id);
            if (studentSubject == null)
            {
                return NotFound(new GeneralServiceResponse { Success = false, Message = "StudentSubject not found" });
            }

            return Ok(studentSubject);
        }

        // ✅ POST: api/StudentSubjects (Bulk add multiple subjects for 1 student)
        [HttpPost]
        public async Task<ActionResult<GeneralServiceResponse>> AddMultipleStudentSubjects([FromBody] StudentSubjectsDtoV2 dto)
        {
            var result = await _studentSubjectService.AddStudentSubject(dto);
            if (result.Success == true)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // DELETE: api/StudentSubjects/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<GeneralServiceResponse>> DeleteStudentSubject(int id)
        {
            var result = await _studentSubjectService.DeleteStudentSubject(id);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GeneralServiceResponse>> UpdateStudentSubject([FromBody] StudentSubjectsDto dto)
        {
            var result = await _studentSubjectService.UpdateStudentSubjects(dto);
            if (result.Success == true)
                return Ok(result);
            return NotFound(result);
        }

        [HttpGet("student")]
        public async Task<ActionResult<IEnumerable<StudentSubjectSpDto>>> GetSubjectsForCurrentStudent(
            [FromQuery] string academicYear,
            [FromQuery] string semester)
        {
            try
            {
                // 1️⃣ Get current logged-in user
                var currentUser = await _authRepository.GetCurrentUserAsync();
                if (currentUser == null)
                {
                    return Unauthorized(new GeneralServiceResponse
                    {
                        Success = false,
                        Message = "User not authenticated"
                    });
                }

                // 2️⃣ Ensure the user is a student
                if (currentUser.Role != UserRole.Student)
                {
                    return Forbid();
                }

                // 3️⃣ Call repository/service method to fetch subjects via stored procedure
                var subjects = await _studentSubjectService.GetSubjectsByStudent(currentUser.Id, academicYear, semester);

                return Ok(subjects);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new GeneralServiceResponse
                {
                    Success = false,
                    Message = $"Failed to fetch subjects: {ex.Message}"
                });
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCurriculum([FromBody] AddCurriculumSubjectDto dto)
        {
            // Validate subject
            var subject = await _context.Subjects.FindAsync(dto.SubjectId);
            if (subject == null)
                return NotFound("Subject not found");

            // Prevent duplicate
            bool exists = await _context.CurriculumSubjects.AnyAsync(c =>
                c.SubjectId == dto.SubjectId &&
                c.YearLevel == dto.YearLevel &&
                c.Semester.ToLower() == dto.Semester.ToLower());

            if (exists)
                return BadRequest("This subject is already assigned to this year & semester.");

            var curriculumSubject = new CurriculumSubject
            {
                SubjectId = dto.SubjectId,
                YearLevel = dto.YearLevel,
                Semester = dto.Semester
            };

            _context.CurriculumSubjects.Add(curriculumSubject);
            await _context.SaveChangesAsync();

            return Ok("Subject successfully added to curriculum");
        }

        //[HttpGet("student/{studentId}/recommended-subjects")]
        //public async Task<IActionResult> GetStudentRecommendedSubjects(int studentId)
        //{
        //    var student = await _context.Users
        //        .AsNoTracking()
        //        .FirstOrDefaultAsync(s => s.Id == studentId);

        //    if (student == null)
        //        return NotFound("Student not found.");

        //    // Get current semester
        //    var currentPeriod = await _context.AcademicPeriods
        //        .AsNoTracking()
        //        .FirstOrDefaultAsync(a => a.IsCurrent == true);

        //    if (currentPeriod == null)
        //        return BadRequest("No current academic period set.");

        //    var yearLevel = student.YearLevel;  // "4" → 4
        //    string semester = currentPeriod.Semester.ToLower();

        //    var subjects = await _context.CurriculumSubjects
        //        .Where(cs =>
        //            cs.YearLevel == yearLevel &&
        //            cs.Semester.ToLower() == semester
        //        )
        //        .Include(cs => cs.Subject)
        //        .Select(cs => new
        //        {
        //            CurriculumSubjectId = cs.Id,
        //            SubjectId = cs.Subject.Id,
        //            SubjectCode = cs.Subject.SubjectCode,
        //            SubjectName = cs.Subject.SubjectName,
        //            Units = cs.Subject.Credits,
        //            Semester = cs.Semester,
        //            YearLevel = cs.YearLevel
        //        })
        //        .ToListAsync();

        //    return Ok(subjects);
        //}

        [HttpGet("student/{studentId}/recommended-subjects")]
        public async Task<IActionResult> GetStudentRecommendedSubjects(int studentId)
        {
            var student = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
                return NotFound("Student not found.");

            // Get current semester
            var currentPeriod = await _context.AcademicPeriods
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.IsCurrent == true);

            if (currentPeriod == null)
                return BadRequest("No current academic period set.");

            var yearLevel = student.YearLevel;
            string semester = currentPeriod.Semester.ToLower();
            string studentDept = student.Department; // <-- ADD

            var subjects = await _context.CurriculumSubjects
                .Include(cs => cs.Subject)
                .Where(cs =>
                    cs.YearLevel == yearLevel &&
                    cs.Semester.ToLower() == semester &&
                    (
                        cs.Subject.Department == studentDept ||    // Match student's department
                        cs.Subject.Department == "General"         // Or general subjects
                    )
                )
                .Select(cs => new
                {
                    CurriculumSubjectId = cs.Id,
                    SubjectId = cs.Subject.Id,
                    SubjectCode = cs.Subject.SubjectCode,
                    SubjectName = cs.Subject.SubjectName,
                    Units = cs.Subject.Credits,
                    Semester = cs.Semester,
                    YearLevel = cs.YearLevel,
                    Department = cs.Subject.Department
                })
                .ToListAsync();

            return Ok(subjects);
        }


        [HttpPost("update")]
        public async Task<IActionResult> UpdateStudentSubjects([FromBody] UpdateStudentSubjectsDTO dto)
        {
            // 1️⃣ Find student
            var student = await _context.Users.FindAsync(dto.StudentId);
            if (student == null)
                return NotFound("Student not found.");

            // 2️⃣ Get latest/current academic period
            var currentPeriod = await _context.AcademicPeriods
                .FirstOrDefaultAsync(a => a.IsCurrent);
            if (currentPeriod == null)
                return BadRequest("No active academic period found.");

            // 3️⃣ Get selected curriculum subjects
            var curriculumSubjects = await _context.CurriculumSubjects
                .Where(cs => dto.CurriculumSubjectIds.Contains(cs.Id))
                .ToListAsync();



            using var transaction = await _context.Database.BeginTransactionAsync();

            var studentDept = student.Department;
            try
            {
                foreach (var cs in curriculumSubjects)
                {

                    var subject = await _context.Subjects.FindAsync(cs.SubjectId);
                    if (subject == null)
                        return BadRequest($"Subject not found: {cs.SubjectId}");

                    if (subject.Department != studentDept && subject.Department != "GENERAL")
                        return BadRequest($"Subject {subject.SubjectName} does not match student’s department ({studentDept}).");

                    // Check if Midterm or Finals already exist for this student, subject, and period
                    var midtermExists = await _context.MidtermGrades.AnyAsync(m =>
                        m.StudentId == dto.StudentId &&
                        m.SubjectId == cs.SubjectId &&
                        m.AcademicPeriodId == currentPeriod.Id
                    );

                    var finalsExists = await _context.FinalsGrades.AnyAsync(f =>
                        f.StudentId == dto.StudentId &&
                        f.SubjectId == cs.SubjectId &&
                        f.AcademicPeriodId == currentPeriod.Id
                    );

                    if (midtermExists || finalsExists)
                    {
                        return BadRequest($"Student already has grades for subject ID {cs.SubjectId} in the current academic period.");
                    }

                    // ✅ ADD new student-subject
                    var studentSubject = new StudentSubject
                    {
                        StudentID = dto.StudentId,
                        SubjectID = cs.SubjectId,
                        AcademicPeriodId = currentPeriod.Id
                    };
                    await _context.StudentSubjects.AddAsync(studentSubject);

                    // 4️⃣ Create default MidtermGrade
                    var midterm = new MidtermGrade
                    {
                        StudentId = dto.StudentId,
                        SubjectId = cs.SubjectId,
                        AcademicPeriodId = currentPeriod.Id,
                        Semester = currentPeriod.Semester,
                        AcademicYear = $"{currentPeriod.StartYear}-{currentPeriod.EndYear}",
                        RecitationScore = 0,
                        AttendanceScore = 0,
                        SEPScore = 0,
                        ProjectScore = 0,
                        PrelimScore = 0,
                        PrelimTotal = 0,
                        MidtermScore = 0,
                        MidtermTotal = 0,
                        TotalMidtermGrade = 0,
                        TotalMidtermGradeRounded = 0,
                        GradePointEquivalent = 5
                    };
                    await _context.MidtermGrades.AddAsync(midterm);

                    // 5️⃣ Create default FinalsGrade
                    var finals = new FinalsGrade
                    {
                        StudentId = dto.StudentId,
                        SubjectId = cs.SubjectId,
                        AcademicPeriodId = currentPeriod.Id,
                        Semester = currentPeriod.Semester,
                        AcademicYear = $"{currentPeriod.StartYear}-{currentPeriod.EndYear}",
                        RecitationScore = 0,
                        AttendanceScore = 0,
                        SEPScore = 0,
                        ProjectScore = 0,
                        FinalsScore = 0,
                        FinalsTotal = 0,
                        TotalQuizScore = 0,
                        ClassStandingTotalScore = 0,
                        ClassStandingAverage = 0,
                        ClassStandingPG = 0,
                        ClassStandingWeightedTotal = 0,
                        QuizPG = 0,
                        QuizWeightedTotal = 0,
                        SEPPG = 0,
                        SEPWeightedTotal = 0,
                        ProjectPG = 0,
                        ProjectWeightedTotal = 0,
                        TotalScoreFinals = 0,
                        OverallFinals = 0,
                        CombinedFinalsAverage = 0,
                        FinalsPG = 0,
                        FinalsWeightedTotal = 0,
                        TotalFinalsGrade = 0,
                        TotalFinalsGradeRounded = 0,
                        GradePointEquivalent = 5
                    };
                    await _context.FinalsGrades.AddAsync(finals);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok("Student subjects and default grades successfully updated for current academic period.");
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        [HttpGet("curriculum")]
        public async Task<IActionResult> GetAllCurriculumSubjects()
        {
            var curriculumSubjects = await _context.CurriculumSubjects
                .Include(cs => cs.Subject)
                .Select(cs => new
                {
                    cs.Id,
                    cs.YearLevel,
                    Semester = char.ToUpper(cs.Semester[0]) + cs.Semester.Substring(1).ToLower(), // Pascal Case
                    SubjectId = cs.Subject.Id,
                    SubjectCode = cs.Subject.SubjectCode,
                    SubjectName = cs.Subject.SubjectName,
                    Credits = cs.Subject.Credits,
                    Department = cs.Subject.Department
                })
                .ToListAsync();

            // Group by YearLevel and Semester
            var grouped = curriculumSubjects
                .GroupBy(cs => new { cs.YearLevel, cs.Semester })
                .Select(g => new
                {
                    YearLevel = g.Key.YearLevel,
                    Semester = g.Key.Semester,
                    SubjectIds = g.Select(s => s.SubjectId).ToList(),
                    Subjects = g.Select(s => new
                    {
                        s.SubjectId,
                        s.SubjectCode,
                        s.SubjectName,
                        s.Credits,
                        s.Department
                    }).ToList()
                })
                .ToList();

            return Ok(grouped);
        }




    }
}
