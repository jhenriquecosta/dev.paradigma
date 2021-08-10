using System;
using System.Collections.Generic;
using System.Text;
using Works.Modules;
using Works.Reflection.Extensions;

namespace Works.Web.Blazor.Configuration
{
    public class WorksWebBlazorModule: WorksModule
    {

        public override void PreInitialize()
        {
            //Adding setting providers
            Configuration.Settings.Providers.Add<WorksBlazorWebSettings>();
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(WorksWebBlazorModule).GetAssembly());
        }

    }
}
