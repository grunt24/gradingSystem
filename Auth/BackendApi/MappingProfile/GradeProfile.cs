using AutoMapper;
using BackendApi.Core.Models;
using BackendApi.Core.Models.Dto;

namespace BackendApi.MappingProfile
{
    public class GradeProfile : Profile
    {
        public GradeProfile()
        {
            CreateMap<MidtermGrade, MidtermGradeDto>()
                .ForMember(dest => dest.StudentFullName, opt => opt.MapFrom(src => src.User.Fullname))
                .ForMember(dest => dest.SubjectTeacher, opt => opt.MapFrom(src => src.Subject.Teacher.Fullname))
                    .ForMember(dest => dest.StudentNumber, opt => opt.MapFrom(src => src.User.StudentNumber))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.SubjectName))
                .ForMember(dest => dest.SubjectCode, opt => opt.MapFrom(src => src.Subject.SubjectCode))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.User.Department))

                .ForMember(dest => dest.QuizWeightedTotal,
               opt => opt.MapFrom(src => src.QuizWeightedTotal))

    // You should map class standing too:
    .ForMember(dest => dest.ClassStandingWeighted,
               opt => opt.MapFrom(src => src.ClassStandingWeightedTotal))

    // same for SEP
    .ForMember(dest => dest.SEPWeighted,
               opt => opt.MapFrom(src => src.SEPWeightedTotal))

    // same for project
    .ForMember(dest => dest.ProjectWeighted,
               opt => opt.MapFrom(src => src.ProjectWeightedTotal))

    // for midterm exam weighted:
    .ForMember(dest => dest.MidtermExamWeighted,
               opt => opt.MapFrom(src => src.MidtermWeightedTotal));

            CreateMap<FinalsGrade, FinalsGradeDto>()
                .ForMember(dest => dest.StudentFullName, opt => opt.MapFrom(src => src.User.Fullname))
                .ForMember(dest => dest.SubjectTeacher, opt => opt.MapFrom(src => src.Subject.Teacher.Fullname))
                    .ForMember(dest => dest.StudentNumber, opt => opt.MapFrom(src => src.User.StudentNumber))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.SubjectName))
                    .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.User.Department))
                .ForMember(dest => dest.SubjectCode, opt => opt.MapFrom(src => src.Subject.SubjectCode));

            CreateMap<MidtermGradeDto, MidtermGrade>();
            CreateMap<FinalsGradeDto, FinalsGrade>();

            // Map the child collections
            CreateMap<QuizList, QuizListDto>();
            CreateMap<ClassStandingItem, ClassStandingItemDto>();

            // And their reverse maps
            CreateMap<QuizListDto, QuizList>();
            CreateMap<ClassStandingItemDto, ClassStandingItem>();

            // NEW MAPPINGS FOR FORMULA
            CreateMap<GradeFormula, GradeFormulaDto>().ReverseMap();
            CreateMap<GradeFormulaItem, GradeFormulaItemDto>().ReverseMap();
            CreateMap<PointGradeAverageFormula, PointGradeAverageFormulaDto>().ReverseMap();

        }
    }
}
