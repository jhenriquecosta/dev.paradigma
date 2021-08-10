using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Works.Web.Blazor.Components.Common
{
    public interface IGridRefreshDataSource
    {
        Task ReloadAsync(object data);
        
    }
}
