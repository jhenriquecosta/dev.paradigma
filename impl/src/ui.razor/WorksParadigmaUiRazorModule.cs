using System.Reflection;
using Works.Modules;

namespace Works.Paradigma
{ 
    [DependsOn(typeof(WorksParadigmaApplicationModule))]
    public class WorksParadigmaUiRazorModule : WorksModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
        public override void PreInitialize()
        {
            base.PreInitialize();
            //Adding custom AutoMapper configuration
        }
    }

}
