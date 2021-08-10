using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using Blazorise;
using JetBrains.Annotations;
using Syncfusion.Blazor.Diagrams;
using Works.Application.Services.Dto;
using Works.Threading;
using Works.Web.Blazor.Common;
using Works.Web.Blazor.Components.Ui.Alert;
using Works.Web.Blazor.Components.Ui.LoadIndicator;
 
using Works.Reports;

namespace Works.Web.Blazor.Components.Ui.Sf.Forms
{

    public abstract class WorksPage : WorksPageBase
    {
       // [Inject] public IReportService ReportService { get; set; }
        [Inject] public IReportViewer ReportViewer { get; set; }
        [Inject] public SweetAlertService Swal { get; set; }
        [Inject] public ILoadingService LoadingTaskService { get; set; }

        
        public  async Task<T> GetWorksResultAndReturnValueAsync<T>(Task<IWorksResultDto<T>> actionTask)
        {
            var result = await actionTask;
            if (result.IsError) await WarningAsync(result.Message);
            return result.Result;
        }
        public async Task<T> GetWorksResultAndReturnValueAsync<T>(Task<WorksResultService<T>> actionTask)
        {
            var result = await actionTask;
            if (!result.IsSuccess) await WarningAsync(result.Message);
            return result.Result;
        }

        //[ItemCanBeNull] 
         //protected async Task<T> GetWorksResultAsync<T>(Func<T> actionFunc)
         //{
         //     var result = actionFunc  
         //}

        public async Task StartTaskAsync(Func<Task> action,
             string mainText=WorksWebBlazorTestsConsts.Ui.LoadingTaskMainText,
             string subText=WorksWebBlazorTestsConsts.Ui.LoadingTaskSubText,
             string contextName=WorksWebBlazorTestsConsts.Ui.LoadingTaskContext)
        {
            await LoadingTaskService.StartTaskAsync(async (task) =>
            {
                await action.Invoke();

            }, contextName, mainText, subText);
        }

        public async Task InfoAsync(string mensagem)
        {
            var alert = new SweetAlertOptions
            {
                Title = "Informação",
                Text = mensagem,
                Icon = SweetAlertIcon.Success,
                ShowConfirmButton = false,
                Timer = 1500
            };
            await Swal.FireAsync(alert);
        }
        public void ShowToast(string mensagem,string heading)
        {
            Toast.ShowInfo(mensagem, heading);
        }
        public async Task ErrorAsync(string mensagem)
        {
            await Swal.FireAsync("Atenção", mensagem, "error");
        }
        public async Task WarningAsync(string mensagem)
        {
            await Swal.FireAsync("Atenção", mensagem, "warning");
        }
        public void ExecuteAsync(Func<Task> task)
        {
            WorksAsync.RunSync(async () => await task.Invoke());
        }
        public void Execute(Func<Task> task)
        {
            WorksAsync.RunSync(async () => await task.Invoke());
        }
        public async Task<bool> QuestionAsync(string text,string title="OWS Projects")
        {
            var objResult = false;
            var result = await Swal.FireAsync(new SweetAlertOptions
            {
                Text = text,
                Title = title, 
                AllowOutsideClick = false,
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
                ConfirmButtonText = "Confirmar",
                CancelButtonText = "Cancelar"
            });

            if (!string.IsNullOrEmpty(result.Value))
            {
                objResult = true;
            }
            else if (result.Dismiss == DismissReason.Cancel)
            {
                await Swal.FireAsync(
                  "Cancelada",
                  "Operação CANCELADA pelo USUÁRIO!",
                  SweetAlertIcon.Error
                  );
            }
            return objResult;
        }

        public void ShowAlert(string alert,AlertMode icon = AlertMode.Info)
        {
            var alertIcon = icon switch
            {
                AlertMode.Error => SweetAlertIcon.Error,
                AlertMode.Success => SweetAlertIcon.Success,
                AlertMode.Warning => SweetAlertIcon.Warning,
                _ => SweetAlertIcon.Info
            };

            var alertMixin = new SweetAlertOptions();
            alertMixin.Icon = alertIcon;
            alertMixin.Toast = true;
            alertMixin.Position = SweetAlertPosition.TopEnd;
            alertMixin.ShowConfirmButton = false;
            alertMixin.Timer = 1500;
            alertMixin.TimerProgressBar = true;
            alertMixin.Title = alert;
           // var res = Swal.Mixin(alertMixin);
            Swal.FireAsync(alertMixin);

        }
    } 
 
}
