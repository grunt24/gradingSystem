using BackendApi.Core.General;
using BackendApi.Core.Models;
using BackendApi.Core.Models.Dto;
using System.Threading.Tasks;
using static GradeCalculationService;

namespace BackendApi.IRepositories
{
    public interface IGradeCalculationService
    {
        //Midterm
        Task<MidtermGradeUploadResult> CalculateAndSaveMidtermGradesAsync(List<MidtermGradeDto> studentGradesDto);

        //addded
        Task<MidtermGrade> CalculateAndSaveSingleMidtermGradeAsync(MidtermGradeDto studentGradeDto);
        Task<ResponseData<IEnumerable<MidtermGradeDto>>> GetMidtermGrades();
        Task<ResponseData<string>> DeleteMidtermGradesAsync(List<int> gradeIds);
        Task<GradeWeights?> GetWeightsAsync();
        //==================================================================================
        //Finals
        Task<FinalsGrade> CalculateAndSaveFinalGradesAsync(FinalsGradeDto studentGradesDto);
        Task<ResponseData<IEnumerable<FinalsGradeDto>>> GetFinalGrades();
        Task<ResponseData<string>> DeleteFinalsGradesAsync(List<int> gradeIds);
        Task<bool> AddQuizToMidtermGradeAsync(int studentId, int midtermGradeId, string label, int? score, int? total);
        //new
        Task<bool> AddQuizToMidtermGradeAsync(int studentId, int gradeId, string label, int score, int total);
        Task<bool> AddClassStandingToMidtermGradeAsync(int studentId, int gradeId, string label, int score, int total);
        Task<MidtermGrade> CalculateAndSaveSingleMidtermGradeAsyncV2(MidtermGradeDto dto);

        Task<MidtermGradeUploadResult> CalculateMidtermGradesForSubjectAsync(int subjectId, int academicPeriodId);
        Task<FinalsGradeUploadResult> CalculateFinalsGradesForSubjectAsync(int subjectId, int academicPeriodId);

        Task<IEnumerable<MidtermGradeDto>> GetGradesBySubjectAndPeriodAsync(int subjectId, int academicPeriodId);
        Task<bool> UpdateMidtermGradeAsync(int studentId, MidtermGradeDto updatedGrade);

        //Finals
        Task<ResponseData<IEnumerable<FinalsGradeDto>>> GetFinalsGradesBySubjectAndPeriodAsync(int subjectId, int academicPeriodId);
        Task<bool> UpdateFinalsGradeAsync(int studentId, FinalsGradeDto updatedGrade);

        //
        Task<MidtermGradeUploadResult> CalculateMidtermGradesForAllSubjectsAsync();
        Task CalculateFinalsGradesForAllSubjectsAsync();

        //11/10/2025
        Task BatchUpdateMidtermGradesAsync(List<MidtermGradeDto> grades);
        //11/10/2025
        Task<MidtermGradeUploadResult> CalculateMidtermGradesBySubjectAsync(int subjectId);
        Task<FinalCourseGrade> CalculateAndSaveFinalCourseGradeAsync(int studentId, int subjectId);
        Task<List<FinalCourseGradeDto>> GetCalculatedFinalsGradesAsync();
    }
}
