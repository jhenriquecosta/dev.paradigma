using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Works.Web.Blazor.Components.Common;

namespace Works.Web.Blazor.Components.Ui.Sf.Toasts
{
    public class BaseWorkToasts : WorksWebBlazorComponent
    {
        [Inject] private IToastService ToastService { get; set; }

        [Parameter] public string InfoClass { get; set; }
        [Parameter] public string InfoIconClass { get; set; }
        [Parameter] public string SuccessClass { get; set; }
        [Parameter] public string SuccessIconClass { get; set; }
        [Parameter] public string WarningClass { get; set; }
        [Parameter] public string WarningIconClass { get; set; }
        [Parameter] public string ErrorClass { get; set; }
        [Parameter] public string ErrorIconClass { get; set; }
        [Parameter] public ToastPosition Position { get; set; } = ToastPosition.TopRight;
        [Parameter] public int Timeout { get; set; } = 5;

        protected string PositionClass { get; set; } = string.Empty;
        internal List<ToastInstance> ToastList { get; set; } = new List<ToastInstance>();
        
        protected override void OnInitialized()
        {
            ToastService.OnShow += ShowToast;

            PositionClass = $"position-{Position.ToString().ToLower()}";
        }

        public void RemoveToast(Guid toastId)
        {
            InvokeAsync(() =>
            {
                var toastInstance = ToastList.SingleOrDefault(x => x.Id == toastId);
                ToastList.Remove(toastInstance);
                StateHasChanged();
            });
        }

        private ToastSettings BuildToastSettings(ToastLevel level, string message, string heading)
        {
            var cssIcon = "e-info toast-icons";
            var cssClass = "e-toast-info";
         
            switch (level)
            {
                case ToastLevel.Info:
                    return new ToastSettings(string.IsNullOrWhiteSpace(heading) ? "Info" : heading,message,cssClass,cssIcon);

                case ToastLevel.Success:
                    cssIcon = "e-success toast-icons";
                    cssClass = "e-toast-success";
                    return new ToastSettings(string.IsNullOrWhiteSpace(heading) ? "Success" : heading, message, cssClass, cssIcon);

                case ToastLevel.Warning:
                    cssIcon = "e-warning toast-icons";
                    cssClass = "e-toast-warning";
                    return new ToastSettings(string.IsNullOrWhiteSpace(heading) ? "Warning" : heading, message, cssClass, cssIcon);

                case ToastLevel.Error:
                    cssIcon = "e-error toast-icons";
                    cssClass = "e-toast-danger";
                    return new ToastSettings(string.IsNullOrWhiteSpace(heading) ? "Error" : heading, message, cssClass, cssIcon);
            }

            throw new InvalidOperationException();
        }

        private void ShowToast(ToastLevel level, string message, string heading)
        {
            var settings = BuildToastSettings(level, message, heading);
            var toast = new ToastInstance
            {
                Id = Guid.NewGuid(),
                TimeStamp = DateTime.Now,
                ToastSettings = settings
            };

            ToastList.Add(toast);

            var timeout = Timeout * 1000;
            var toastTimer = new Timer(timeout);
            toastTimer.Elapsed += (sender, args) => { RemoveToast(toast.Id); };
            toastTimer.AutoReset = false;
            toastTimer.Start();

            StateHasChanged();
        }
    }
}
