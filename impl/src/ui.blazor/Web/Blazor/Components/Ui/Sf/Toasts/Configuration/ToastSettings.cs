namespace Works.Web.Blazor.Components.Ui.Sf.Toasts
{
    public class ToastSettings
    {
        public ToastSettings(string heading, string message,string cssClass = "e-toast-info", string iconClass = "e-info toast-icons",string additionalClasses="")
        {
            Heading = heading;
            Message = message;
            CssClass = cssClass;       
            IconClass = iconClass;
            AdditionalClasses = additionalClasses;
        }

        public string Heading { get; set; }
        public string Message { get; set; }
        public string CssClass { get; set; }
        public string AdditionalClasses { get; set; }
        public string IconClass { get; set; }
    }
}
