
namespace BLL.Profiles
{
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            // CreateTicketDto -> Ticket
            CreateMap<CreateTicketDto, Ticket>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<TicketStatusEnum>(src.Status)))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<TicketTypeEnum>(src.Type)));

            // Ticket -> TicketDto
            CreateMap<Ticket, TicketDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // Ticket -> TicketDetailsDto
            CreateMap<Ticket, TicketDetailsDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.TaskTitle, opt => opt.MapFrom(src => src.Task.Title))
                .ForMember(dest => dest.TaskDto, opt => opt.MapFrom(src => src.Task))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));

            // UpdateTicketDto -> Ticket
            CreateMap<UpdateTicketDto, Ticket>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<TicketStatusEnum>(src.Status)))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<TicketTypeEnum>(src.Type))).ReverseMap();

            // Ticket -> UpdateTicketDto (لو هتحتاجه في حالة Edit مثلا)
            CreateMap<Ticket, UpdateTicketDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));
        }
    }
}
