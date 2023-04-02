//using System;
//using La_Grazia.Database;
//using Microsoft.EntityFrameworkCore;

//namespace La_Grazia.Infastructure.Configurations
//{
//    public static class DatabaseConfigurations
//    {
//        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
//        {
//            services.AddDbContext<DataContext>(o =>
//            {
//                o.UseSqlServer(configuration.GetConnectionString("NatavanMAC"));
//            });
//        }
//    }
//}