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
    public static class RenderTreeBuilderExtensions 
    {
        
        public static RenderTreeBuilder AddField(this RenderTreeBuilder builder,RenderFragment renderFragment, IFluentColumn Size)
        {
            var sequence = 0;
            builder.OpenComponent<Field>(sequence);
            builder.AddAttribute(++sequence, "ColumnSize",Size);
            builder.AddAttribute(++sequence, "ChildContent", renderFragment); 
            builder.CloseComponent();
            return builder;
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
