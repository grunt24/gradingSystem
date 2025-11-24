using BackendApi.Core.General;
using BackendApi.Core.Models;
using BackendApi.Core.Models.Dto;

namespace BackendApi.IRepositories
{
    public interface IStudentSubjectService
    {
        Task<IEnumerable<StudentSubjectGroupedDto>> GetAllStudentSubjects();
        Task<StudentSubject> GetStudentSubjectById(int id);
        Task<GeneralServiceResponse> AddStudentSubject(StudentSubjectsDtoV2 studentSubjectDto);
        Task<GeneralServiceResponse> DeleteStudentSubject(int id);
        Task<GeneralServiceResponse> UpdateStudentSubjects(StudentSubjectsDto dto);
        Task<IEnumerable<StudentSubjectSpDto>> GetSubjectsByStudent(int studentId, string academicYear, string semester);
        }
}
