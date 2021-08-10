using System.Reflection;
using Works.Modules;

namespace Works.Paradigma
{
    [DependsOn(typeof(WorksParadigmaDataAccessModule))]

    public class WorksParadigmaApplicationModule : WorksModule
    {
        public override void PreInitialize()
        {

            Configuration.Navigation.Providers.Add<WorksParadigmaApplicationMenu>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
        public override void PostInitialize()
        {
           
            // WorksSeedTypes.Initialize(IocManager, typeof(WorksImobDomainModule).GetAssembly());
           // WorksNHibernate.Initialize(IocManager);

        }
    }
}
