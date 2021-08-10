
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Works.Web.Blazor.Components.Common;
using Works.Web.Enums;

namespace Works.Web.Blazor.Components.Ui.Sf.Buttons
{
    public class BaseWorkButton : WorksWebBlazorComponent
    {

        string varMaterialDesignIcon;
        string varIcon = "mdi-send";

        [Parameter] public EventCallback OnClick { get; set; }
        [Parameter] public ButtonStyle Style { get; set; } = ButtonStyle.None;

        [Parameter] public string Tooltip { get; set; } = "";
        [Parameter] public bool IsSubmit { get; set; } = false;
        [Parameter] public bool IsDisable { get; set; } = false;
        [Parameter] public bool IsPrimary { get; set; }
        [Parameter] public bool IsToogle { get; set; } = false;
        [Parameter] public bool UseMaterialIcon { get; set; } = true;
        [Parameter] public string Icon { get; set; }

        protected override Task OnParametersSetAsync()
        {
            base.OnParametersSetAsync();
            HtmlAttributes = new Dictionary<string, object>();
            if (UseMaterialIcon)
            {
                if (Icon.IsNullOrEmpty()) Icon = varIcon;
                varMaterialDesignIcon = $"mdi {Icon} mdi-18px";
                CssIcon = varMaterialDesignIcon;
            }
            if (IsSubmit)
            {
                HtmlAttributes.Add("type", "submit");

            }
            if (!Tooltip.IsNullOrEmpty())
            {
                HtmlAttributes.Add("title", Tooltip);
            }
            if (this.Style != ButtonStyle.None)
            {
                var _style = WorksEnum.GetDescription(typeof(ButtonStyle), Style);
                CssClass = _style;
            }
            return Task.CompletedTask;
        }
    }
}
