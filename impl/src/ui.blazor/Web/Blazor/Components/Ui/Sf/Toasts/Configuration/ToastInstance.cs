using System;

namespace Works.Web.Blazor.Components.Ui.Sf.Toasts
{
    internal class ToastInstance
    {
        public Guid Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public ToastSettings ToastSettings { get; set; }
    }
}
