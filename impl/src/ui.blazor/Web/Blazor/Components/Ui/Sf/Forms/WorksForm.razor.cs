
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Buttons;
using Works.Web.Blazor.Components.Common;
using Works.Web.Enums;

namespace Works.Web.Blazor.Components.Ui.Sf.Forms
{
    public class BaseWorkForm : WorksWebBlazorComponent
    {
        protected SfButton SfButtonSave { get; set; }
        protected SfButton SfButtonDelete { get; set; }
        protected SfButton SfButtonExit { get; set; }

        [Parameter] public string SubTitle { get; set; } 
        [Parameter] public System.Action FormSubmit { get; set; }
     
        [Parameter] public RenderFragment Buttons { get; set; }
        [Parameter] public WorksPageModelAction PageModelAction { get; set; }

        [Parameter] public System.Action OnResetClick { get; set; }
        [Parameter] public EventCallback OnSaveClick { get; set; }
        [Parameter] public EventCallback OnDeleteClick { get; set; }

        [Parameter] public string CloseButtonText { get; set; } = "CANCELAR";
        [Parameter] public string SaveButtonText { get; set; } = "CONFIRMAR";
        [Parameter] public string DeleteButtonText { get; set; } = "DELETAR";

        [Parameter] public bool SaveButtonDisabled { get; set; } = false;
        [Parameter] public bool DeleteButtonDisabled { get; set; } = false;



        protected void OnSaveButtonClick(MouseEventArgs e) => OnSaveClick.InvokeAsync(e);

        protected void OnFormSubmit(System.EventArgs e)
        {
            FormSubmit?.Invoke();
        }
         
        public void ButtonSaveDisabled(bool buttonDisabled)
        {
            if (SfButtonSave.IsNull()) return;
            SfButtonSave.Disabled = buttonDisabled;
        }

        protected void OnResetButtonClick()
        {
            OnResetClick();
        }
      
       
    }
}
