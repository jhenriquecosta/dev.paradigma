
using Blazorise;
using Microsoft.AspNetCore.Components;
using Works.Web.Blazor.Components.Common;

namespace Works.Web.Blazor.Components.Ui.Sf.Panels
{
    public class BaseWorkPanel : WorksWebBlazorComponent
    {
        [Parameter] public IFluentColumn SizeColumn { get; set; } = ColumnSize.IsFull.OnDesktop.IsFull.OnMobile;
        [Parameter] public IFluentSpacing SizeMargin { get; set; } = Margin.Is1.OnAll; 
        [Parameter] public string Title { get; set; }
        [Parameter] public string SubTitle { get; set; }
        [Parameter] public string UrlImage { get; set; } = "";
        [Parameter] public RenderFragment TopBar { get; set; }
        [Parameter] public RenderFragment BottomBar { get; set; }
    }
}

 