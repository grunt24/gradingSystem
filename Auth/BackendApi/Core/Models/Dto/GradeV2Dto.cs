namespace BackendApi.Core.Models.Dto
{
    public class GradeFormulaDto
    {
        public int Id { get; set; }

        // Academic Year + Semester
        public int? AcademicPeriodId { get; set; }

        // Year Level (1st yr, 2nd yr...)
        public string? YearLevel { get; set; }

        // Subject-based formulas (optional)
        public int? SubjectId { get; set; }

        public bool IsActive { get; set; } = true;
    }

    //This will create Quizzes = 30%, Class Standing = 25%, SEP = 5%, Project = 10%, Midterm Exam = 30% one by 1
    public class GradeFormulaItemDto
    {
        public int Id { get; set; }

        public int GradeFormulaId { get; set; }

        // Dynamic Component Name (Admin-controlled)
        public string ComponentName { get; set; } = string.Empty;

        // Example: 0.30 for 30%
        public decimal Weight { get; set; }

        // If true, this component only applies to subjects that match SubjectType
        public bool IsSubjectSpecific { get; set; } = false;

        // Example: "English", "Math", etc.
        public string? SubjectType { get; set; }
        // NEW: Should the system generate a separate table (list items)?
        public bool HasMultipleItems { get; set; } = false;

        // NEW: Name of the list-table for dynamic linking
        public string? ListTableName { get; set; }

    }

        public class PointGradeAverageFormulaDto
        {
            public int Id { get; set; }
            public int GradeFormulaId { get; set; }

            public decimal PercentageMultiplier { get; set; }
            public decimal BasePoints { get; set; }
        }

}
