using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Works.Application.Services.Dto;
using Works.Reports;
using Works.Web.Blazor.Components.Ui.Alert;
using Works.Web.Blazor.Components.Ui.LoadIndicator;
using Works.Web.Blazor.Components.Ui.Sf.Toasts;

namespace Works.Web.Blazor.Components.Common
{
    public  class WorksPageComponent : OwningComponentBase
    {
        [Inject] public IToastService Toast { get; set; }
        [Inject] public IJSRuntime JsRuntime { get; set; }
        [Inject] public HttpClient HttpClient { get; set; }
        [Inject] public NavigationManager NavManager { get; set; }
        [Inject] private IExceptionHelper ExceptionHelper { get; set; }
        [Inject] public IReportViewer ReportViewer { get; set; }
        [Inject] public SweetAlertService Swal { get; set; }
        [Inject] public ILoadingService LoadingTaskService { get; set; }
        protected string IndicatorContext { get; set; } = "pageinitcontext";

        public string ErrorMessage { get; set; }
        public bool LoadFailed { get; set; }
        public bool IsVisible { get; set; } = false;
        public bool IsInitialized { get; set; } = false;


        protected void HandleException(Exception ex)
        {
            if (ex is UnauthorizedAccessException)
            {
                NavManager.NavigateTo("/unauthorized");
            }
            else
            {
                ExceptionHelper.StoreException(ex);
                NavManager.NavigateTo("/error");
            }
        }

        public virtual void ShouldRenderize()
        {
            InvokeAsync(StateHasChanged);
        }
        ~WorksPageComponent()
        {
            this.Dispose(false);
        }
        public void Dispose()
        {
            this.Dispose(true);
        }

        public async Task StartTaskAsync(Func<Task> action,
             string mainText = WorksWebBlazorTestsConsts.Ui.LoadingTaskMainText,
             string subText = WorksWebBlazorTestsConsts.Ui.LoadingTaskSubText,
             string contextName = WorksWebBlazorTestsConsts.Ui.LoadingTaskContext)
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
        public void ShowToast(string mensagem, string heading)
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
      
        public async Task<bool> QuestionAsync(string text, string title = "OWS Projects")
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

        public void ShowAlert(string alert, AlertMode icon = AlertMode.Info)
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


        public void ExecuteAsync(Func<Task> task)
        {
            WorksAsync.RunSync(async () => await task.Invoke());
        }
        public void Execute(Func<Task> task)
        {
            WorksAsync.RunSync(async () => await task.Invoke());
        }
        public async Task<T> GetDataResult<T>(Task<IWorksResultDto<T>> actionTask)
        {
            var result = await actionTask;
            if (result.IsError) await WarningAsync(result.Message);
            return result.Result;
        }
        public async Task<T> GetDataResult<T>(Task<WorksResultService<T>> actionTask)
        {
            var result = await actionTask;
            if (!result.IsSuccess) await WarningAsync(result.Message);
            return result.Result;
        }

       
    }
}
