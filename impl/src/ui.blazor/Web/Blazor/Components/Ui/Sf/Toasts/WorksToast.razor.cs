using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Notifications;
using Works.Web.Blazor.Components.Common;
using Works.Web.Blazor.Components.Ui.Sf.Toasts;

namespace Works.Web.Blazor.Components.Ui.Sf.Toasts
{
    public class BaseWorkToast : WorksWebBlazorComponent
    {
        public SfToast SelfComponent { get; set; }
        [CascadingParameter] private WorksToasts ToastsContainer { get; set; }
        [Parameter] public Guid ToastId { get; set; }
        [Parameter] public ToastSettings ToastSettings { get; set; }
      
        [Parameter] public int Timeout { get; set; } = 1500;


        public bool ShowCloseBtn = true;
        public bool ShowProgressBtn = false;
        public bool ShowNewestOnTop = true;
        public bool ShowButtons { get; set; } = false;
        public bool Checked { get; set; } = true;
        public ToastEasing ShowEasing { get; set; }
        public ToastEasing HideEasing { get; set; }
        public ToastEffect ShowEffect { get; set; }
        public ToastEffect HideEffect { get; set; }
        public string EasingValue { get; set; } = "ease";
        public string AnimationValue { get; set; } = "FadeZoomIn";
        public string HideEasingValue { get; set; } = "ease";
        public string HideAnimationValue { get; set; } = "FadeZoomOut";

        public string TimeoutValue { get; set; } = "5000";
        public int ShowDuration = 400;
        public int HideDuration = 400;
        public string ShowDurationtValue = "400";
        public string HideDurationtValue = "400";

        public List<ToastButton> ToastButtons;
        protected void OnCreated(object args)
        {
            
            OnShow();
        }
        public void OnShow()
        {
            this.StateHasChanged();
            this.SelfComponent.Show();
        }
        protected void Close()
        {
            ToastsContainer.RemoveToast(ToastId);
        }

    }
}