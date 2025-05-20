
namespace BLL.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            // CreateCommentDto → Comment
            CreateMap<CreateCommentDto, Comment>();

            // UpdateCommentDto ←→ Comment
            CreateMap<UpdateCommentDto, Comment>().ReverseMap();

            // Comment → CommentDto (عرض بسيط)
            CreateMap<Comment, CommentDto>();

            // Comment → CommentDetailsDto
            CreateMap<Comment, CommentDetailsDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName)) // لو عندك FullName في ApplicationUser
                .ForMember(dest => dest.TicketDto, opt => opt.MapFrom(src => src.Ticket)); // لازم TicketDto يكون مضاف مسبقاً في AutoMapper

        }
    }
}
