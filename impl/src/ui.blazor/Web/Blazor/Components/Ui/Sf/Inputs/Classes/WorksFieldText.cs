using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Inputs;
using System.Threading.Tasks;
using Works.Extensions;

namespace Works.Web.Blazor.Components.Ui.Sf.Inputs
{
    public class WorksFieldText : WorksFieldText<string>
    {

    }

    public class WorksFieldText<TValue> : WorksInputBase<TValue> 
    {
        protected SfTextBox objEditText { get; set; }
       

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var sequence = 0;
            if (FieldSize != null)
            {
                builder.AddField(BuildControl(), FieldSize);
            }
            else
            {

                builder.OpenComponent<SfTextBox>(sequence);
                builder.AddAttribute(++sequence, "Placeholder", Caption);
                builder.AddAttribute(++sequence, "Value", Value);
                builder.AddAttribute(++sequence, "Locale", Locale);
                builder.AddAttribute(++sequence, "ShowClearButton", ShowClearButton);
                builder.AddAttribute(++sequence, "Readonly", !AllowEdit);
                builder.AddAttribute(++sequence, "CssClass", CssClass);
                builder.AddAttribute(++sequence, "Multiline", Multiline);
                builder.AddAttribute(++sequence, "FloatLabelType", FloatLabel);
                builder.AddAttribute(++sequence, "OnChange", RuntimeHelpers.TypeCheck(EventCallback.Factory.Create<ChangeEventArgs>(this, OnValueChanged)));
                builder.AddComponentReferenceCapture(++sequence, (value) =>
                {
                    objEditText = value as SfTextBox;
                });
                builder.CloseComponent();
            }
        }
        private RenderFragment BuildControl()
        {
            var sequence = 0;
            RenderFragment render = builder =>
            {
                builder.OpenComponent<SfTextBox>(sequence);
                builder.AddAttribute(++sequence, "Placeholder", Caption);
                builder.AddAttribute(++sequence, "Value", Value);
                builder.AddAttribute(++sequence, "Locale", Locale);
                builder.AddAttribute(++sequence, "ShowClearButton", ShowClearButton);
                builder.AddAttribute(++sequence, "Readonly", !AllowEdit);
                builder.AddAttribute(++sequence, "CssClass", CssClass);
                builder.AddAttribute(++sequence, "Multiline", Multiline);
                builder.AddAttribute(++sequence, "FloatLabelType", FloatLabel);
                builder.AddAttribute(++sequence, "OnChange", RuntimeHelpers.TypeCheck(EventCallback.Factory.Create<ChangeEventArgs>(this , OnValueChanged)));          
                builder.AddComponentReferenceCapture(++sequence, (value) =>
                {
                    objEditText = value as SfTextBox;
                });
                builder.CloseComponent();
            };
            return render;
        }
    }

//    @inherits BaseWorkTextEdit
//<Field ColumnSize="@Size">
//    <SfTextBox OnChange = "OnValueChanged"
//               OnBlur= "OnBlurFired"
//               Locale= "@Locale"
//               ShowClearButton= "@ShowClearButton"
//               @ref= "objEditText"
//               Readonly= "@(!AllowEdit)"
//               CssClass= "@CssClass"
//               @bind-Value= "@Value"
//               Placeholder= "@Caption"
//               Multiline= "@Multiline"
//               FloatLabelType= "@FloatLabel" >
//    </ SfTextBox >
//</ Field >

}
