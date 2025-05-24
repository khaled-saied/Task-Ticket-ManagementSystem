
namespace BLL.Profiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            // CreateProjectDto → Project
            CreateMap<CreateProjectDto, Project>();

            // UpdateProjectDto ←→ Project
            CreateMap<UpdateProjectDto, Project>().ReverseMap();

            // Project → ProjectDto (عرض بسيط)
            CreateMap<Project, ProjectDto>();

            // Project → ProjectDetailsDto
            CreateMap<Project, ProjectDetailsDto>()
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.User.UserName)) 
                .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.Tasks));
        }
    }
}
