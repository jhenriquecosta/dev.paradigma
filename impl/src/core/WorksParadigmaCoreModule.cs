using System.Reflection;
using Works.Modules;

namespace Works.Paradigma
{

    public class WorksParadigmaCoreModule : WorksModule
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
