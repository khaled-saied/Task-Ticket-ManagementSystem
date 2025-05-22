using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Services.Interfaces;
using DAL.Repositories;

namespace BLL.Services.Classes
{
    class ServiceMangerWithFactorDelegate(Func<IProjectService> ProjectServiceFactory,
                                                 Func<ITicketService> TicketServiceFactory,
                                                 Func<ITaskService> TaskServiceFactory,
                                                 Func<ICommentService> CommentServiceFactory) : IServiceManger
    {

        public ICommentService CommentService => CommentServiceFactory.Invoke();

        public ITicketService TicketService => TicketServiceFactory.Invoke();

        public IProjectService ProjectService => ProjectServiceFactory.Invoke();

        public ITaskService TaskService => TaskServiceFactory.Invoke();
    }
}
