
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Works.Web.Blazor.Common;

namespace Works.Web.Blazor.Components.Ui.LoadIndicator
{
    public abstract class LoadingIndicatorParadigmaBase : ComponentBase
    {
        [Parameter]
        public ITaskStatus CurrentTask { protected get; set; }

        public Task CallStateHasChanged() => InvokeAsync(StateHasChanged);
    }
}
