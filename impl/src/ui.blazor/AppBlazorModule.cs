using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Works.Application.Configuration;
using Works.Configuration;
using Works.Configuration.Startup;
using Works.IO;
using Works.Modules;
using Works.Reflection.Extensions;

 
 
using Works.DataAccess;
using NHibernate.Tool.hbm2ddl;
using System.Data.Common;
using System;
 
using FluentNHibernate.Cfg.Db;
using Works.NHibernate;
using System.Reflection;
 
using Works.AspNetCore;
using Works.AspNetCore.Configuration;
 

using Works.Blazor;
using Works.FluentNHibernate;

using Works.Ui.Blazor;
using Works.Paradigma;
using Castle.MicroKernel.Registration;

namespace Works
{

    
    [DependsOn(typeof(WorksParadigmaUiRazorModule),             
               typeof(WorksAspNetCoreModule),
               typeof(WorksUiBlazorModule),             
               typeof(WorksNHibernateModule))]
    public class AppBlazorModule : WorksModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _appConfiguration;

        public AppBlazorModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();


        }

        public override void PreInitialize()
        {

            SetUpDatabase();
            SetUpFolders();
            IocManager.Register<IAppConfigurationAccessor, DefaultAppConfigurationAccessor>();
          

            Configuration.Modules.WorksAspNetCore().CreateControllersForAppServices(typeof(AppBlazorModule).GetAssembly());
            Configuration.Modules.WorksNHibernate().FluentConfiguration.Build();
            //uso do cache

        }



        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AppBlazorModule).GetAssembly());
        }

        public override void PostInitialize()
        {
           // SetAppFolders();
           IocManager.Resolve<ApplicationPartManager>().AddApplicationPartsIfNotAddedBefore(typeof(AppBlazorModule).Assembly);
           WorksNHibernate.Initialize(IocManager);
        }

        private void SetUpDatabase()
        {
            var configAcessor = new DefaultAppConfigurationAccessor().Configuration;
            var databaseInit = configAcessor.GetSection("Works").GetSection("DatabaseInitializer").Get<WorksDbInitializer>();
            databaseInit.ConnectionString = configAcessor.GetConnectionString(databaseInit.ConnectionStringName);
            IocManager.IocContainer.Register(Component.For<IWorksDbInitializer>().Instance(databaseInit).LifestyleSingleton());
            
            Configuration.DefaultNameOrConnectionString = databaseInit.ConnectionString;
            Configuration.UnitOfWork.Timeout = TimeSpan.FromMinutes(30);
            Configuration.UnitOfWork.IsTransactional = false;

        }
        private void SetUpFolders()
        {
            var appFolders = WorksWebApp.CreateFolders(_env.ContentRootPath);
            IocManager.IocContainer.Register(Component.For<IAppFolders>().Instance(appFolders).LifestyleSingleton());


        }

    }
}
