using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using Syncfusion.Blazor.InPlaceEditor;
using System;
using System.Threading.Tasks;
using Works.Extensions;

namespace Works.Web.Blazor.Components.Ui.Sf.Inputs
{


    public class WorksInputInPlace : WorksInputBase<object>
    {
        protected SfInPlaceEditor<object> _internalControl;
      
        [Parameter] public RenderMode Mode { get; set; } = RenderMode.Inline;
        protected InputType Type { get; set; } = Syncfusion.Blazor.InPlaceEditor.InputType.Text;
        protected object Model { get; set; } = new TextBoxModel();
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Init();
            Model = GetTypeModel(Value.GetType());
        }
        protected void Init()
        {
            
        }

        protected object GetTypeModel(Type type)
        {
            if (type == typeof(string))
            {
                if (Format.IsEmpty())
                {
                    Type = InputType.Text;
                    return new TextBoxModel() { Placeholder = Caption, ShowClearButton = ShowClearButton, Readonly = !AllowEdit, Multiline = Multiline, FloatLabelType = FloatLabel };
                }
                else
                {
                    Type = InputType.Mask;
                    return new MaskedTextBoxModel() { Placeholder = Caption, Mask = Format, ShowClearButton = ShowClearButton, Readonly = !AllowEdit, FloatLabelType = FloatLabel };
                }
                    
            }
            if (type == typeof(decimal?) || type == typeof(decimal))
            {
                if (Format.IsEmpty()) this.Format = "C2";
                if (Decimals == 0) this.Decimals = 2;
                Type = InputType.Numeric;
                return new NumericTextBoxModel<decimal?>() { Placeholder = Caption, Format = Format, Decimals = Decimals, ShowClearButton = ShowClearButton, Readonly = !AllowEdit, FloatLabelType = FloatLabel };
            }
            if (type == typeof(DateTime?) || type == typeof(DateTime))
            {
                Type = InputType.Date;
                return new DatePickerModel() { Placeholder = Caption, ShowClearButton = ShowClearButton, Readonly = !AllowEdit, FloatLabelType = FloatLabel };

            }
          //  if (type == InputType.DateTime) return new DateTimePickerModel<TValue>() { Placeholder = Caption, ShowClearButton = ShowClearButton, Readonly = !AllowEdit, FloatLabelType = FloatLabel };
          //  if (type == InputType.DateRange) return new DateRangePickerModel() { Placeholder = Caption, ShowClearButton = ShowClearButton, Readonly = !AllowEdit, FloatLabelType = FloatLabel };          
          //  if (type == InputType.Time) return new TimePickerModel<string>() { Placeholder = Caption, ShowClearButton = ShowClearButton, Readonly = !AllowEdit, FloatLabelType = FloatLabel };

          ////  if (type == InputType.Color) return new TextBoxModel() { Placeholder = Caption, ShowClearButton = ShowClearButton, Readonly = !AllowEdit, Multiline = Multiline, FloatLabelType = FloatLabel };

          //  if (type == InputType.ComboBox) return new TextBoxModel() { Placeholder = Caption, ShowClearButton = ShowClearButton, Readonly = !AllowEdit, Multiline = Multiline, FloatLabelType = FloatLabel };
          //  if (type == InputType.DropDownList) return new TextBoxModel() { Placeholder = Caption, ShowClearButton = ShowClearButton, Readonly = !AllowEdit, Multiline = Multiline, FloatLabelType = FloatLabel };
          //  if (type == InputType.AutoComplete) return new TextBoxModel() { Placeholder = Caption, ShowClearButton = ShowClearButton, Readonly = !AllowEdit, Multiline = Multiline, FloatLabelType = FloatLabel };
          //  if (type == InputType.MultiSelect) return new TextBoxModel() { Placeholder = Caption, ShowClearButton = ShowClearButton, Readonly = !AllowEdit, Multiline = Multiline, FloatLabelType = FloatLabel };

            else return new TextBoxModel() { Placeholder = Caption, ShowClearButton = ShowClearButton, Readonly = !AllowEdit, Multiline = Multiline, FloatLabelType = FloatLabel };
           
                
        }
        
         //builder.AddAttribute(++sequence, "OnChange", RuntimeHelpers.TypeCheck(EventCallback.Factory.Create<ChangeEventArgs>(this, OnValueChanged)));


        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
               var sequence = 0;
               
                builder.OpenComponent<SfInPlaceEditor<object>>(sequence);
                builder.AddAttribute(++sequence, "Mode", Mode);
                builder.AddAttribute(++sequence, "Type", Type);
                builder.AddAttribute(++sequence, "Model", Model);

                builder.AddAttribute(++sequence, "Value", Value);
                builder.AddAttribute(++sequence, "Locale", Locale);

                builder.AddAttribute(++sequence, "CssClass", CssClass);
            
                   builder.AddComponentReferenceCapture(++sequence, (value) =>
                {
                    _internalControl = value as SfInPlaceEditor<object>;
                });
                builder.CloseComponent();
            
        }
       
    }
}

