using Blazorise;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Inputs;
using System.Threading.Tasks;
using Works.Web.Blazor.Components.Common;

namespace Works.Web.Blazor.Components.Ui.Sf
{
    public abstract class WorksSfComponentBase : WorksWebBlazorComponent
    {
        [CascadingParameter] public IFluentColumn Size { get; set; } 
        [Parameter] public IFluentSpacing SetMargin { get; set; } = Blazorise.Margin.Is2.OnY;
        [Parameter] public FloatLabelType FloatLabel { get; set; } = FloatLabelType.Auto;
        [Parameter] public IFluentColumn FieldSize { get; set; } //= ColumnSize.IsFull.OnDesktop.IsFull.OnMobile;
        [Parameter] public string FieldHelp { get; set; }
        [Parameter] public bool ShowClearButton { get; set; } = true;
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (Size == null) Size = FieldSize;
        }

    }
    public abstract class WorksSfComponentBase<TValue> : WorksSfComponentBase
    {
        [Parameter] public TValue Value { get; set; }

    }
}
