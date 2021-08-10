using System.Collections.Generic;

namespace Works.Web.Blazor.Ui.SweetDialog
{
    public class SweetAlertQueueResult
    {
        public IEnumerable<string> Value { get; set; }

        public DismissReason? Dismiss { get; set; }
    }
}
