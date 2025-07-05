using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Services.AttachmentServices.AttachmentServices;
using BLL.Services.Interfaces;
using DAL.Repositories;

namespace BLL.Services.Classes
{
    public class ServiceMangerWithFactorDelegate(Func<IProjectService> ProjectServiceFactory,
                                                 Func<ITicketService> TicketServiceFactory,
                                                 Func<ITaskService> TaskServiceFactory,
                                                 Func<ICommentService> CommentServiceFactory,
                                                 Func<IUserService> UserServiceFactory) : IServiceManger
    {

        public ICommentService CommentService => CommentServiceFactory.Invoke();
        public ITicketService TicketService => TicketServiceFactory.Invoke();
        public IProjectService ProjectService => ProjectServiceFactory.Invoke();
        public ITaskService TaskService => TaskServiceFactory.Invoke();
        public IUserService UserService => UserServiceFactory.Invoke();
    }
}
