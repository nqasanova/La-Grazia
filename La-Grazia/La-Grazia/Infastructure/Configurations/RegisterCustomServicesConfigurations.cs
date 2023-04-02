//using System;
//using La_Grazia.Database;
//using La_Grazia.Database.Models;
//using La_Grazia.Services;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;

//namespace La_Grazia.Infastructure.Configurations
//{
//    public class RegisterCustomServicesConfigurations
//    {
//        public RegisterCustomServicesConfigurations(IConfiguration configuration)
//        {
//            Configuration = configuration;
//        }

//        public IConfiguration Configuration { get; }

//        public void ConfigureServices(IServiceCollection services)
//        {
//            services.AddControllersWithViews().AddNewtonsoftJson(options =>
//            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

//            services.AddIdentity<User, IdentityRole>(options =>
//            {
//                options.Password.RequireNonAlphanumeric = false;
//            }).AddDefaultTokenProviders().AddEntityFrameworkStores<DataContext>();

//            services.AddScoped<LayoutService>();
//            services.AddScoped<IEmailService, EmailService>();
//            services.AddHttpContextAccessor();
//            services.AddSession();
//        }
//    }
//}