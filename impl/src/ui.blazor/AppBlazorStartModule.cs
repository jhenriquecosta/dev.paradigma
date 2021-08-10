using System.IO;
using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Works.Configuration.Startup;
using Works.Modules;

using Works.Application.Configuration;
using Works.Net.Mail;
using Works.Dependency;
using System;
 
using Works.IO;
using Microsoft.AspNetCore.Hosting;
 
using System.Data.SQLite;
using Castle.MicroKernel.Registration;
using System.Data.Common;
using Works.DataAccess;
using System.Data.SqlClient;

using Works.AutoMapper;
 
using Works.Configuration;


namespace Works
{ 
    
    [DependsOn(typeof(WorksParadigmaUiRazorModule))]
    public class AppBlazorStartModule : WorksModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _appConfiguration;
 
        public AppBlazorStartModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }
        public override void Initialize()
        {



        }
        public override void PreInitialize()
        {
            IocManager.Register<IAppConfigurationAccessor, DefaultAppConfigurationAccessor>();          
            Configuration.UnitOfWork.Timeout = TimeSpan.FromMinutes(30);
            Configuration.UnitOfWork.IsTransactional = false;
 
            SetUpDatabase();
            SetUpFolders();

        }
        private void SetUpDatabase()
        {
            var configAcessor = new DefaultAppConfigurationAccessor().Configuration;
            var databaseInit = configAcessor.GetSection("Works").GetSection("DatabaseInitializer").Get<DatabaseInitializer>();
            databaseInit.ConnectionString = configAcessor.GetConnectionString(databaseInit.ConnectionStringName);
            IocManager.IocContainer.Register(Component.For<IDatabaseInitializer>().Instance(databaseInit).LifestyleSingleton());
            Configuration.DefaultNameOrConnectionString = databaseInit.ConnectionString;

        }
        private void SetUpFolders()
        {
            var appFolders = WorksWebApp.CreateFolders(_env.ContentRootPath);
            IocManager.IocContainer.Register(Component.For<IAppFolders>().Instance(appFolders).LifestyleSingleton());
  

        }
      

       
    }
}
