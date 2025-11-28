namespace BackendApi.Core.Models
{
    public class FinalCourseGrade
    {
        public int Id { get; set; }
        public decimal ComputedTotalMidtermGrade { get; set; }
        public int RoundedTotalMidtermGrade { get; set; }
        public decimal ComputedTotalFinalGrade { get; set; }
        public int RoundedTotalFinalGrade { get; set; }
        public decimal ComputedFinalCourseGrade { get; set; }
        public int RoundedFinalCourseGrade { get; set; }
        public int AcademicYearId { get; set; }
        public decimal GradePointEquivalent { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }

    }
}
