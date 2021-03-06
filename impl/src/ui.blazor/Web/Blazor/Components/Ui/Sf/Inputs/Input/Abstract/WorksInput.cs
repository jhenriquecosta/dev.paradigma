using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Buttons;
using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.Inputs;
using System;
using System.Threading.Tasks;
using Works.Extensions;

namespace Works.Web.Blazor.Components.Ui.Sf.Inputs
{


    public class WorksInput<TValue> : WorksInputAbstract<TValue>
    {

    }
    public abstract class WorksInputAbstract<TType> : WorksInputBase<TType>
    {
         
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var sequence = 0;
            var type = typeof(TType);

            var attrCaption = "Placeholder";
            var attrValue = "Value";
            var attrDisabled = "Readonly";
            var disabledProperty = !AllowEdit;

            if (type.IsNumber())
            {
                if (type.IsNumberDecimal())
                {
                    if (Format.IsEmpty()) this.Format = "C2";
                    if (Decimals == 0) this.Decimals = 2;
                }
                builder.OpenComponent<SfNumericTextBox<TType>>(sequence);
                builder.AddAttribute(++sequence, "Format", Format);
                builder.AddAttribute(++sequence, "Decimals", Decimals);
                builder.AddAttribute(++sequence, "ShowSpinButton", ShowSpinButton);
                builder.AddAttribute(++sequence, "ChildContent", (RenderFragment)((changeEvent) =>
                {
                    changeEvent.OpenComponent<NumericTextBoxEvents<TType>>(++sequence);
                    changeEvent.AddAttribute(++sequence, "ValueChange", RuntimeHelpers.TypeCheck(EventCallback.Factory.Create<Syncfusion.Blazor.Inputs.ChangeEventArgs<TType>>(this, OnValueChanged)));
                    changeEvent.CloseComponent();
                }));
            }
            else if (type.IsDate())
            {
                builder.OpenComponent<SfDatePicker<TType>>(sequence);              
                builder.AddAttribute(++sequence, "Format", Format);
                ShowClearButton = false;
            }
            else if (type.IsBoolean())
            {
                builder.OpenComponent<SfCheckBox<TType>>(sequence);
                builder.AddAttribute(++sequence, "onchange", RuntimeHelpers.TypeCheck(EventCallback.Factory.Create<ChangeEventArgs>(this, OnValueChanged)));
                attrCaption = "Label";
                attrValue = "Checked";
                attrDisabled = "Disabled";
               
            }

            else
            {
                if (Format.IsEmpty())
                {
                    builder.OpenComponent<SfTextBox>(sequence);
                    builder.AddAttribute(++sequence, "Multiline", Multiline);
                    builder.AddAttribute(++sequence, "Input", RuntimeHelpers.TypeCheck(EventCallback.Factory.Create<InputEventArgs>(this, OnValueChanged)));
                }

                else
                {
                    builder.OpenComponent<SfMaskedTextBox>(sequence);
                    builder.AddAttribute(++sequence, "Mask", Format);
                    if (!PromptChar.IsEmpty()) builder.AddAttribute(++sequence, "PromptChar", PromptChar);
                    if (CustomCharacters != null) builder.AddAttribute(++sequence, "CustomCharacters", CustomCharacters);
                    builder.AddAttribute(++sequence, "ValueChange", RuntimeHelpers.TypeCheck(EventCallback.Factory.Create<MaskChangeEventArgs>(this, OnValueChanged)));
                }
            }
            
            builder.AddAttribute(++sequence, attrCaption, Caption);
            builder.AddAttribute(++sequence, attrValue, Value);
            builder.AddAttribute(++sequence, attrDisabled, disabledProperty);
            builder.AddAttribute(++sequence, "Locale", Locale);
            if (!type.IsBoolean())
            {
                builder.AddAttribute(++sequence, "ShowClearButton", ShowClearButton);
                builder.AddAttribute(++sequence, "CssClass", CssClass);
                builder.AddAttribute(++sequence, "FloatLabelType", FloatLabel);
            }
            if (type.IsDate())
            {
                builder.AddAttribute(++sequence, "OnChange", RuntimeHelpers.TypeCheck(EventCallback.Factory.Create<ChangeEventArgs>(this, OnValueChanged)));
                builder.AddAttribute(++sequence, "ChildContent", (RenderFragment)((changeEvent) =>
                {
                    changeEvent.OpenComponent<DatePickerEvents<TType>>(0);
                    changeEvent.AddAttribute(1, "Cleared", RuntimeHelpers.TypeCheck(EventCallback.Factory.Create<Syncfusion.Blazor.Calendars.ClearedEventArgs>(this, OnValueCleared)));
                    changeEvent.AddAttribute(2, "ValueChange", RuntimeHelpers.TypeCheck(EventCallback.Factory.Create<ChangedEventArgs<TType>>(this, OnValueChanged)));                  
                    changeEvent.CloseComponent();
                }));
            }
            builder.AddComponentReferenceCapture(++sequence, (value) =>
            {
                InternalCmp = value;
            });

            builder.CloseComponent();

        }

    }
    //public static class ObjectTypeExtensions
    //{
    //    public static bool IsDate(this Type type)
    //    {
    //        if (type == typeof(DateTime))
    //            return true;
    //        if (type == typeof(DateTime?))
    //            return true;
    //        return false;
    //    }
    //    public static bool IsNumber(this Type type)
    //    {
    //        if (IsNumberDecimal(type) || IsNumberInt(type)) return true;
    //        return false;
    //    }
    //    public static bool IsNumberDecimal(this Type type)
    //    {
    //        if (type == typeof(double))
    //            return true;
    //        if (type == typeof(double?))
    //            return true;
    //        if (type == typeof(decimal))
    //            return true;
    //        if (type == typeof(decimal?))
    //            return true;
    //        if (type == typeof(float))
    //            return true;
    //        if (type == typeof(float?))
    //            return true;
    //        return false;
    //    }
    //    public static bool IsNumberInt(this Type type)
    //    {
    //        if (type == typeof(int))
    //            return true;
    //        if (type == typeof(int?))
    //            return true;
    //        if (type == typeof(short))
    //            return true;
    //        if (type == typeof(short?))
    //            return true;
    //        if (type == typeof(long))
    //            return true;
    //        if (type == typeof(long?))
    //            return true;
    //        return false;
    //    }
    //}

}

