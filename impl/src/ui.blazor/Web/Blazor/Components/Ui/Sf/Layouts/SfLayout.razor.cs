using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel;

namespace Works.Web.Blazor.Components.Ui.Sf.Layouts
{
    public enum SFClass
    {
        [Description("sb-ej2")]
        EJ2,
        [Description("sample-browser e-view sf-new")]
        Main,
        
        [Description("sb-content e-view")]
        Panel,
        [Description("sb-left-pane e-view")]
        PanelLeft,
        [Description("sb-left-pane-header")]
        PanelLeftHeader,
        [Description("sb-right-pane e-view")]
        PanelRight,
        [Description("sb-desktop-wrapper")]
        PanelCenter,

        [Description("sb-header e-view")]
        Header,
        [Description("sb-header-top")]
        HeaderTop,
        [Description("sb-header-left sb-left sb-table")]
        HeaderLeft,
        [Description("sb-header-right sb-right sb-table")]
        HeaderRight,

        [Description("sb-mobile-overlay")]
        MobileOverlay,
        [Description("sb-mobile-logo")]
        MobileLogo,
        [Description("sb-mobile-header-buttons")]
        MobileHeaderButton,
        [Description("sb-mobile-header-about")]
        MobileHeaderAbout,

        [Description("sb-name")]
        Name,
        [Description("sb-home")]
        Home,
        [Description("sb-home-link")]
        HomeLink,
        [Description("sb-home-text")]
        HomeText,

        [Description("sb-left-footer")]
        LeftFooter,
        [Description("sb-left-footer-links")]
        LeftFooterLinks,
    }
    public class LayoutBase : ComponentBase
    {
        [Parameter] public SFClass Class { get; set; }
        [Parameter] public string ID { get; set; }
        [Parameter] public string Role { get; set; }
        [Parameter] public bool HomeIcon { get; set; }
        [Parameter] public bool Hide { get; set; } = false;
        [Parameter] public EventCallback OnClick { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public RenderFragment ComponentContent { get; set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            ComponentContent = builder =>
            {
                BuilderComponent(builder);
            };
        }

        private void BuilderComponent(RenderTreeBuilder builder)
        {
            var seq = 0;
            var sbHide = "sb-hide";
            var sbIconHome = "sb-icons sb-icon-Home";
            var divClass = Class.GetDescription<SFClass>();
            if (HomeIcon)
            {
                divClass = $"{divClass} {sbIconHome}";
            }
            if (Hide)
            {
                divClass = $"{divClass} {sbHide}";
            }
            builder.OpenElement(seq++, "div");
            
                if (!this.ID.IsNullOrWhiteSpace())
                {
                    builder.AddAttribute(seq++, "id", this.ID);
                }
                builder.AddAttribute(seq++, "class", divClass);
                if (!this.Role.IsNullOrEmpty())
                {
                    builder.AddAttribute(seq++, "role", this.Role);
                }
                builder.AddAttribute(seq++, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this,OnClick));
                if (ChildContent != null) builder.AddContent(seq++, ChildContent);

            builder.CloseElement();

        }


    }
}
