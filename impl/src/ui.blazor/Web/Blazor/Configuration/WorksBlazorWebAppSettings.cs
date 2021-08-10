using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Works.Configuration;

namespace Works.Web.Blazor.Configuration
{
    public class WorksBlazorWebSettings : SettingProvider
    {
     
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(WorksBlazorWebConsts.Settings.ServerSide, "true")
            }; 
        }
    }
}
