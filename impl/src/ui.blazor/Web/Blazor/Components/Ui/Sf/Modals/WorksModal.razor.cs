
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Popups;
using Works.Web.Blazor.Components.Common;
using Works.Extensions;

namespace Works.Web.Blazor.Components.Ui.Sf.Modals
{
    public class BaseWorkModal : WorksWebBlazorComponent
    {
       
        protected bool isVisible = true;
        protected SfDialog ejsWinModal;
        protected DialogAnimationSettings dialogAnnimation { get; set; } = new DialogAnimationSettings { Effect = DialogEffect.Zoom };
        [Parameter] public RenderFragment Body { get; set; }
        [Parameter] public RenderFragment Footer { get; set; }
        [Parameter] public string Title { get; set; }
        [Parameter] public string SubTitle { get; set; }
        [Parameter] public string UrlImage { get; set; } = "";
        [Parameter] public string Header { get; set; } = "";
        [Parameter] public string Target { get; set; } = "#target";

        [Parameter] public bool IsVisible { get; set; } = false;
        [Parameter] public bool IsActive { get; set; } = false;

        [Parameter] public bool ShowBtnOkCancel { get; set; } = false;
        [Parameter] public bool ShowBtnOk { get; set; } = false;
        [Parameter] public bool ShowBtnCancel { get; set; } = false;

        [Parameter] public EventCallback OnBtnOkClick { get; set; }
        [Parameter] public EventCallback OnBtnCancelClick { get; set; }


       
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (Width.IsNullOrEmpty())
            {
                Width = "700px";
            }
        }
        public void Show()
        {
            this.IsActive=true;
            this.IsVisible = true;
            this.ejsWinModal.Show();
            this.ShouldRenderize();

        }

        public override void ShouldRenderize()
        {
            base.ShouldRenderize();
        }
        public void Hide()
        {
            this.IsActive = false;
            this.IsVisible = false;
            this.ejsWinModal.Hide();
            this.ShouldRenderize();
        }

    }

}
