//using System;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using AspNetCore.IServiceCollection.AddIUrlHelper;
//using La_Grazia.Infastructure.Configurations;

//namespace La_Grazia.Infastructure.Extentions
//{
//    public static class ServiceCollectionExtensions
//    {
//        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
//        {
//            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//                .AddCookie(o =>
//                {
//                    o.Cookie.Name = "Identity";
//                    o.ExpireTimeSpan = TimeSpan.FromMinutes(20);
//                    o.LoginPath = "/auth/login";
//                    o.AccessDeniedPath = "/admin/auth/login";
//                });

//            services.AddHttpContextAccessor();

//            services.AddUrlHelper();


//            services.ConfigureMvc();

//            services.ConfigureDatabase(configuration);

//            //services.ConfigureOptions(configuration);

//            //services.RegisterCustomServices(configuration);
//        }
//    }
//}