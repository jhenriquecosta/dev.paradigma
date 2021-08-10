using Microsoft.AspNetCore.Components;
using System;
using Works.Web.Blazor.Components.Common;

namespace Works.Web.Blazor.Components.Ui.Sf.Forms
{


    public abstract class WorksPageBase : WorksWebBlazorForm
    {
       
        [Inject] private IExceptionHelper ExceptionHelper { get; set; }
      
        public string ErrorMessage { get; set; }
        public bool LoadFailed { get; set; }
        public bool IsVisible { get; set; } = false;
        public bool IsInitialized { get; set; } = false;
        

        protected void HandleException(Exception ex)
        {
            if (ex is UnauthorizedAccessException)
            {
                NavManager.NavigateTo("/unauthorized");
            }
            else
            {
                ExceptionHelper.StoreException(ex);
                NavManager.NavigateTo("/error");
            }
        }

        
    }
}
