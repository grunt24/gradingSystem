using BackendApi.Context;
using BackendApi.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcademicPeriodsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public AcademicPeriodsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentAcademicPeriod()
        {
            var current = await _dbContext.AcademicPeriods
                .FirstOrDefaultAsync(p => p.IsCurrent);

            if (current == null)
                return NotFound("No current academic period set.");

            return Ok(new
            {
                startYear = current.StartYear,
                endYear = current.EndYear,
                semester = current.Semester,
                academicYear = $"{current.StartYear}-{current.EndYear}"
            });
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllAcademicPeriod()
        {
            var periods = await _dbContext.AcademicPeriods
                .OrderByDescending(p => p.StartYear)
                .ThenByDescending(p => p.Semester)
                .ToListAsync();

            return Ok(periods);
        }

        //[HttpPost("set-current")]
        //public async Task<IActionResult> SetCurrentAcademicPeriod([FromBody] AcademicPeriodDto dto)
        //{
        //    // Optional: validate input, check for overlaps

        //    // Unset previous current
        //    var existing = await _dbContext.AcademicPeriods
        //        .Where(p => p.IsCurrent)
        //        .ToListAsync();

        //    existing.ForEach(p => p.IsCurrent = false);

        //    var newPeriod = new AcademicPeriod
        //    {
        //        StartYear = dto.StartYear,
        //        EndYear = dto.EndYear,
        //        Semester = dto.Semester,
        //        IsCurrent = true
        //    };

        //    _dbContext.AcademicPeriods.Add(newPeriod);
        //    await _dbContext.SaveChangesAsync();

        //    return Ok(new { message = "Academic period set successfully." });
        //}

        [HttpPost("set-current")]
        public async Task<IActionResult> SetCurrentAcademicPeriod([FromBody] AcademicPeriodDto dto)
        {
            // Optional: validate input, check for overlaps

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                // Unset previous current
                var existing = await _dbContext.AcademicPeriods
                    .Where(p => p.IsCurrent)
                    .ToListAsync();

                existing.ForEach(p => p.IsCurrent = false);

                // Add new academic period
                var newPeriod = new AcademicPeriod
                {
                    StartYear = dto.StartYear,
                    EndYear = dto.EndYear,
                    Semester = dto.Semester,
                    IsCurrent = true
                };

                _dbContext.AcademicPeriods.Add(newPeriod);
                await _dbContext.SaveChangesAsync();

                // ✅ Reset all student assigned subjects
                var allStudentSubjects = await _dbContext.StudentSubjects.ToListAsync();
                if (allStudentSubjects.Any())
                {
                    _dbContext.StudentSubjects.RemoveRange(allStudentSubjects);
                }

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { message = "Academic period set successfully and all student subjects reset." });
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAcademicPeriod(int id, [FromBody] AcademicPeriodDto dto)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var period = await _dbContext.AcademicPeriods.FindAsync(id);
                if (period == null)
                    return NotFound("Academic period not found.");

                // Update period properties
                period.StartYear = dto.StartYear;
                period.EndYear = dto.EndYear;
                period.Semester = dto.Semester;

                // If updating to current, unset previous current periods
                if (!period.IsCurrent)
                {
                    var currentPeriods = await _dbContext.AcademicPeriods
                        .Where(p => p.IsCurrent)
                        .ToListAsync();
                    currentPeriods.ForEach(p => p.IsCurrent = false);
                    period.IsCurrent = true;
                }

                _dbContext.AcademicPeriods.Update(period);
                await _dbContext.SaveChangesAsync();

                // Optionally reset all student subjects and grades if this period becomes current
                if (period.IsCurrent)
                {
                    var allStudentSubjects = await _dbContext.StudentSubjects.ToListAsync();
                    if (allStudentSubjects.Any())
                        _dbContext.StudentSubjects.RemoveRange(allStudentSubjects);

                    await _dbContext.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return Ok(new { message = "Academic period updated successfully." });
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }



        //[HttpGet("get-all-ay-semester")]
        //public async Task<IActionResult> GetAllAcademicPeriod()
        //{
        //    var academicPeriods =  await _dbContext.AcademicPeriods.ToListAsync();
        //    return Ok(academicPeriods);
        //}


        public class AcademicPeriodDto
        {
            public int StartYear { get; set; }
            public int EndYear { get; set; }
            public string Semester { get; set; }
        }
    }
}
