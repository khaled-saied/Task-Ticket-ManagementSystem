
namespace BLL.Profiles
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            // CreateTaskDto -> TaskK
            CreateMap<CreateTaskDto, TaskK>();

            // UpdateTaskDto <=> TaskK (two-way + enum conversion)
            CreateMap<UpdateTaskDto, TaskK>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<TaskStatusEnum>(src.Status)))
                .ReverseMap()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // TaskDto <=> TaskK (two-way + enum conversion)
            CreateMap<TaskK, TaskDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<TaskStatusEnum>(src.Status)));

            // TaskDetailsDto -> TaskK (one-way, no ReverseMap)
            CreateMap<TaskK, TaskDetailsDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}
