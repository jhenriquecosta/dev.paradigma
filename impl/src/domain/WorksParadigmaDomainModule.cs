using System.Reflection;
using Works.AutoMapper;
using Works.Modules;

namespace Works.Paradigma
{
    [DependsOn(typeof(WorksAutoMapperModule))]
    public class WorksParadigmaDomainModule : WorksModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration


            Configuration.Modules.WorksAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }

}
