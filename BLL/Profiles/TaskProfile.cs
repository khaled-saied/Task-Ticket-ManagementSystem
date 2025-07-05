
namespace BLL.Profiles
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            // CreateTaskDto -> TaskK
            CreateMap<CreateTaskDto, TaskK>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));

            // UpdateTaskDto <=> TaskK (two-way + enum conversion)
            CreateMap<UpdateTaskDto, TaskK>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<TaskStatusEnum>(src.Status)))
                .ReverseMap()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // TaskDto <=> TaskK (two-way + enum conversion)
            CreateMap<TaskK, TaskDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest=> dest.AssignedToUserId,opt => opt.MapFrom(src => src.User.UserName != null? src.User.UserName: "Unassigned"))
                .ReverseMap()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<TaskStatusEnum>(src.Status)));

            // TaskDetailsDto -> TaskK (one-way, no ReverseMap)
            CreateMap<TaskK, TaskDetailsDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.CtreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(src => src.User.UserName != null ? src.User.UserName : "Unassigned"));
        }
    }
}
