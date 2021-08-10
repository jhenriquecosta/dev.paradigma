using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http;
using Works.Web.Blazor.Components.Ui.Sf.Toasts;

namespace Works.Web.Blazor.Components.Common
{
    public abstract class WorksWebBlazorForm : OwningComponentBase
    {
        [Inject] public IToastService Toast { get; set; }
        [Inject] public IJSRuntime JsRuntime { get; set; }
        [Inject] public HttpClient HttpClient { get; set; }
        [Inject] public NavigationManager NavManager { get; set; }

        public virtual void OnChanged()
        {
            InvokeAsync(StateHasChanged);
        }

        ~WorksWebBlazorForm()
        {
            this.Dispose(false);
        }
        public void Dispose()
        {
            this.Dispose(true);
        }
    }
}
