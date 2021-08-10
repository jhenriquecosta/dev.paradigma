using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Works
{
    public static class AppBlazorSettings
    {
        public static IEnumerable<Assembly> GetAssemblies()
        {
            var pattern = "Ui.Razor";
            var assemblies = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                             where assembly.FullName.Contains(pattern)
                             select assembly;
            return assemblies;
        }
    }
}
