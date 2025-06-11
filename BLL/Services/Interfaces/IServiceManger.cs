using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IServiceManger
    {
        ICommentService CommentService { get; }
        ITicketService TicketService { get; }
        IProjectService ProjectService { get; }
        ITaskService TaskService { get; }
        IUserService UserService { get; }
    }
}
