using BLL;
using BLL.Exceptions;
using BLL.Profiles;
using DAL.Data.DbContexts;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ticket_ManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        { 
            var builder = WebApplication.CreateBuilder(args);

            
            #region Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddApplicationServices();

            //Add Service Of Authentication
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(op =>
            {
                op.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            #endregion

            var app = builder.Build();

            #region Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStaticFiles(); 

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Register}/{id?}")
                .WithStaticAssets();
            #endregion

            app.Run();
        }
    }
}
