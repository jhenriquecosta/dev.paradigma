using log4net;
using log4net.Appender;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection;
using Works.AspNetCore.Dependency;

using Works.Dependency;
using Works.Helpers;
using Works.IO;
using Works.Log4Net;

namespace Works
{
    public class Program
    {
        public static int Main(string[] args)
        {
            CurrentDirectoryHelpers.SetCurrentDirectory();
           
            var Log = GetLogger();
           
            try
            {
                //IdentityServer4 seed should be happening here but because of this bug https://github.com/aspnet/AspNetCore/issues/12349
                //the seeding is not implemented here.
                Log.Info("Starting web server host");

                // Per: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host
                //    The Web Host is no longer recommended for web apps and remains 
                //    available only for backward compatibility.
                var host = CreateHostBuilder(args).Build();

                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal("Host terminated unexpectedly", ex);               
                return 1;
            }
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)

                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseConfiguration(new ConfigurationBuilder().AddCommandLine(args).Build())
                   .UseSetting(WebHostDefaults.DetailedErrorsKey, "true")
                   .UseStaticWebAssets()
                   .UseStartup<Startup>();
                }).UseCastleWindsor(IocManager.Instance.IocContainer);
            
        private static void InitDatabase()
        {
            // Initialize the database
            //var scopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();
            //using (var scope = scopeFactory.CreateScope())
            //{
            //    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            //    if (db.Database.EnsureCreated())
            //    {
            //        SeedData.Initialize(db);
            //    }
            //}
        }
        private static ILog GetLogger()
        {
            var path =Environment.CurrentDirectory;
            var file = $"webserver.log";
            
            return WorksLog4Net.Instance.Initialize(path, typeof(Program).FullName, file).Log;
        }

    }
}
