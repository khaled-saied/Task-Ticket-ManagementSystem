using BLL;
using BLL.Exceptions;
using BLL.Profiles;
using DAL.Data.DbContexts;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ticket_ManagementSystem.Settings;
using Ticket_ManagementSystem.Utilities;

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
                options.UseLazyLoadingProxies();
            });

            builder.Services.AddApplicationServices();

            //Add Service Of Authentication
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(op =>
            {
                op.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true; // This will reset the expiration time on each request, keeping the user logged in as long as they are active.
                options.ExpireTimeSpan = TimeSpan.FromDays(2);
            });

            #region External Login With Google
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                            .AddCookie(op =>
                            {
                                op.LoginPath = "/Account/Login";
                                op.AccessDeniedPath = "/Account/AccessDenied";
                                op.SlidingExpiration = true; // This will reset the expiration time on each request, keeping the user logged in as long as they are active.
                                op.ExpireTimeSpan = TimeSpan.FromDays(2);
                            })
                            .AddGoogle(op =>
                            {
                                op.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                                op.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                            });
            #endregion

            //Configure MailSettings
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

            //Configure SmsSettings
            builder.Services.Configure<SmsSettings>(builder.Configuration.GetSection("Twilio"));

            builder.Services.AddScoped<IMailService, MailService>();

            builder.Services.AddScoped<ISmsService, SmsService>();

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
                pattern: "{controller=Account}/{action=Login}/{id?}")
                .WithStaticAssets();
            #endregion

            app.Run();
        }
    }
}
