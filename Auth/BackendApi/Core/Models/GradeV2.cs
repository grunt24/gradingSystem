namespace BackendApi.Core.Models
{
    public class GradeV2
    {

    }

    public class GradeFormula
    {
        public int Id { get; set; }

        // Academic Year + Semester
        public int? AcademicPeriodId { get; set; }
        public AcademicPeriod AcademicPeriod { get; set; }

        // Year Level (1st yr, 2nd yr...)
        public string? YearLevel { get; set; }

        // Subject-based formulas (optional)
        public int? SubjectId { get; set; }
        public Subject? Subject { get; set; }

        // List of dynamic components
        public ICollection<GradeFormulaItem> Items { get; set; } = new List<GradeFormulaItem>();

        public bool IsActive { get; set; } = true;
    }

    //This will create Quizzes = 30%, Class Standing = 25%, SEP = 5%, Project = 10%, Midterm Exam = 30% one by 1
    public class GradeFormulaItem
    {
        public int Id { get; set; }

        public int GradeFormulaId { get; set; }
        public GradeFormula GradeFormula { get; set; }

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
    public class PointGradeAverageFormula
    {
        public int Id { get; set; }

        // FK to GradeFormula (Year Level / Academic Period)
        public int GradeFormulaId { get; set; }
        public GradeFormula GradeFormula { get; set; }

        // The multiplier used in the formula (ex: 70)
        public decimal PercentageMultiplier { get; set; }

        // The base points added (ex: 30)
        public decimal BasePoints { get; set; }

        // Example final computation:
        // Final = ROUND((RawScore / TotalScore * PercentageMultiplier) + BasePoints, 2)
    }

}
