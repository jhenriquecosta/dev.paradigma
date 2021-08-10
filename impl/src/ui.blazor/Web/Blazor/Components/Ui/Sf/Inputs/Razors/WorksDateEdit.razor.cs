using System.Threading.Tasks;
using Syncfusion.Blazor.Calendars;

namespace Works.Web.Blazor.Components.Ui.Sf.Inputs
{
    public class WorkBaseDateEdit<TValue> : WorksInputBase<TValue>
    {
       protected SfDatePicker<TValue> objDateEdit { get; set; }
       protected async Task OnValueChange(ChangedEventArgs<TValue> args)
       {
           await ValueChanged.InvokeAsync(args.Value);
           StateHasChanged();

       }
    }
}
