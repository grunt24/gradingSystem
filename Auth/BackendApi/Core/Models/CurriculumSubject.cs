using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BackendApi.Core.Models
{
    public class CurriculumSubject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SubjectId { get; set; }

        [JsonIgnore]
        public Subject? Subject { get; set; }

        [Required]
        public string? YearLevel { get; set; }   // 1, 2, 3, 4

        [Required, MaxLength(20)]
        public string Semester { get; set; } = null!; // First, Second, Summer
    }

    public class AddCurriculumSubjectDto
    {
        public int SubjectId { get; set; }
        public string YearLevel { get; set; }   // 1,2,3,4
        public string Semester { get; set; } // "First" / "Second"
    }

    public class UpdateStudentSubjectsDTO
    {
        public int StudentId { get; set; }
        public List<int> CurriculumSubjectIds { get; set; }
    }


}
