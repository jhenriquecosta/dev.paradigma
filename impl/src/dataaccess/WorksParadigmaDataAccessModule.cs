using System.Reflection;
using Works.Configuration.Startup;
using Works.DataAccess;
using Works.FluentNHibernate;
using Works.Modules;
using Works.NHibernate;

namespace Works.Paradigma
{

    [DependsOn(typeof(WorksParadigmaDomainModule),
               typeof(WorksNHibernateModule))]
    public class WorksParadigmaDataAccessModule : WorksModule
    {
        public override void PreInitialize()
        {
            base.PreInitialize();
            //Adding custom AutoMapper configuration
            Configuration.Modules.WorksNHibernate().FluentConfiguration.AddTypesFrom(typeof(WorksParadigmaDomainModule).Assembly);
          //  Configuration.Modules.WorksNHibernate().FluentConfiguration.AddTypesFrom(typeof(WorksImobDomainModule).Assembly);



        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
        public override void PostInitialize()
        {
            var seed = IocManager.Resolve<IWorksDbInitializer>().SeedEnumerations;
            if (seed)
            {
                InitAndSeedDatabase.Run(IocManager);
            }

        }
    }


}
