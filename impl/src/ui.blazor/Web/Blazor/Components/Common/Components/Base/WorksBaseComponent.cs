using System;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Syncfusion.Blazor;
using Works.Web.Blazor.Components.Ui.Sf.Toasts;
using System.ComponentModel;

namespace Works.Web.Blazor.Components.Common
{   
    public abstract class WorksBaseComponent : OwningComponentBase
    {
        protected ElementReference ElementRef { get; set; }        
        protected object InternalCmp { get; set; }
        public bool IsInitialized { get; set; } = false;
        public bool IsVisible { get; set; } = false;
        public virtual void ShouldRenderize()
        {
            InvokeAsync(StateHasChanged);
        }

        ~WorksBaseComponent()
        {
            this.Dispose(false);
        }
        public void Dispose()
        {
            this.Dispose(true);
        }
        
       
      
    }
}
