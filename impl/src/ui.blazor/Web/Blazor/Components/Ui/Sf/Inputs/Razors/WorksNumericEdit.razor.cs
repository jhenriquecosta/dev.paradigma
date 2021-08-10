
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Syncfusion.Blazor.Inputs;

namespace Works.Web.Blazor.Components.Ui.Sf.Inputs
{
    public class WorkBaseNumericEdit<TValue> : WorksInputBase<TValue>
    {
       protected SfNumericTextBox<TValue> objNumberEdit { get; set; }
     
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (typeof(TValue) == typeof(decimal?) || typeof(TValue) == typeof(decimal))
            {
                if (!Format.IsNullOrWhiteSpace()) this.Format = "C2";
                if (Decimals == 0) this.Decimals = 2;
            }
            
        }
    }
}
