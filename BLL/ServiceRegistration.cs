using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using BLL.Profiles;
using BLL.Services.AttachmentServices;
using BLL.Services.AttachmentServices.AttachmentServices;
using BLL.Services.Classes;
using BLL.Services.Interfaces;
using DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Add AutoMapper
            services.AddAutoMapper(typeof(ProjectProfile).Assembly);

            //Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IServiceManger, ServiceMangerWithFactorDelegate>();

            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<Func<IProjectService>>(sp=> () => sp.GetRequiredService<IProjectService>());

            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<Func<ITicketService>>(sp => () => sp.GetRequiredService<ITicketService>());

            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<Func<ITaskService>>(sp => () => sp.GetRequiredService<ITaskService>());

            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<Func<ICommentService>>(sp => () => sp.GetRequiredService<ICommentService>());

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<Func<IUserService>>(sp => () => sp.GetRequiredService<IUserService>());

            services.AddTransient<IAttachmentServices, AttachmentService>();

            return services;
        }
    }
}
