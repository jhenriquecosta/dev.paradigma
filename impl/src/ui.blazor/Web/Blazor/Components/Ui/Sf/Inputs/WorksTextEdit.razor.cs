using Syncfusion.Blazor.Inputs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Works.Web.Blazor.Ui.Sf.Inputs
{
    public class BaseWorkTextEdit : WorksInputBase<string>
    {
        protected SfTextBox objEditText { get; set; }
        [Parameter] public bool Multiline { get; set; } = false;
        
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            this.Value ??= string.Empty;
        }

    }
}
