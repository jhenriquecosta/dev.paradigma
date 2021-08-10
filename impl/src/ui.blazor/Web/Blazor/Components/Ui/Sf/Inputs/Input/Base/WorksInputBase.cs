using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Buttons;
using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.Inputs;
using System.Threading.Tasks;
using Works.Extensions;

namespace Works.Web.Blazor.Components.Ui.Sf.Inputs
{
    public abstract class WorksInputBase<TValue> : WorksSfComponentBase<TValue>
    {
       

        [Parameter] public LabelPosition LabelPosition { get; set; } = LabelPosition.Before;
        [Parameter] public object CustomCharacters { get; set; }
        [Parameter] public string PromptChar { get; set; }

        [Parameter] public bool Multiline { get; set; } = false;
        [Parameter] public string Format { get; set; } = string.Empty;
        [Parameter] public int Decimals { get; set; } = 0;
        [Parameter] public bool ShowSpinButton { get; set; } = false;
        [Parameter] public EventCallback<TValue> ValueChanged { get; set; }
        [Parameter] public EventCallback<TValue> OnTextChanged { get; set; }
        [Parameter] public EventCallback<Microsoft.AspNetCore.Components.Web.FocusEventArgs> OnBlurFired { get; set; }
        
        protected virtual Task OnValueChanged(Microsoft.AspNetCore.Components.ChangeEventArgs e)
        {
            this.Value = e.Value.ChangeType<TValue>();            
            OnTextChanged.InvokeAsync(this.Value);
            return ValueChanged.InvokeAsync(this.Value);

        }
        protected virtual Task OnValueChanged(MaskChangeEventArgs e)
        {
            this.Value = e.Value.ChangeType<TValue>();           
            return ValueChanged.InvokeAsync(this.Value);

        }
      
        protected virtual Task OnValueChanged(Syncfusion.Blazor.Inputs.ChangeEventArgs<TValue> e)
        {
            this.Value = e.Value.ChangeType<TValue>();
            return ValueChanged.InvokeAsync(this.Value);

        }
        protected virtual Task OnValueChanged(InputEventArgs e)
        {
            this.Value = e.Value.ChangeType<TValue>();
            return ValueChanged.InvokeAsync(this.Value);
        }
        protected virtual Task OnValueCleared(Syncfusion.Blazor.Calendars.ClearedEventArgs args)
        {
            this.Value = default(TValue);            
            return ValueChanged.InvokeAsync(this.Value);
        }
        protected virtual Task OnValueChanged(ChangedEventArgs<TValue> e)
        {
            this.Value = e.Value.ChangeType<TValue>();
            return ValueChanged.InvokeAsync(this.Value);

        }
    }
}
