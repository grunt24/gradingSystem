namespace BackendApi.Core.Models.Dto
{
    public class SubjectDto
    {
        public string? SubjectName { get; set; }
        public string? SubjectCode { get; set; }
        public string? Description { get; set; }
        public int Credits { get; set; }
        public int? TeacherId { get; set; }
        public string? Department { get; set; }
    }

    public class SubjectWithTeacherDto
    {
        public int Id { get; set; }
        public string? SubjectName { get; set; }
        public string? SubjectCode { get; set; }
        public string? Description { get; set; }
        public int Credits { get; set; }
        public string? TeacherName { get; set; }
        public string? SubjectDepartment { get; set; }


    }

    public class StudentSubjectSpDto
    {
        public int StudentSubjectId { get; set; }    // ss.Id
        public string? StudentFullName { get; set; } // s.Fullname
        public int SubjectId { get; set; }           // subj.Id
        public string? SubjectName { get; set; }     // subj.SubjectName
        public string? AcademicYear { get; set; }    // CAST(ap.StartYear AS VARCHAR) + '-' + CAST(ap.EndYear AS VARCHAR)
        public string? Semester { get; set; }        // ap.Semester
    }
}
