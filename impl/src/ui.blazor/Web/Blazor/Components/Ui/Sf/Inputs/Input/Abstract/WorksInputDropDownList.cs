using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using Syncfusion.Blazor.DropDowns;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Works.Application.Services.Dto;
using Works.Extensions;

namespace Works.Web.Blazor.Components.Ui.Sf.Inputs
{
    public abstract class WorksInputDropDownList<TValue, TItem> : WorksInputDropDownList<TValue, TItem, TItem,TValue>
    {

    }
    public abstract class WorksInputDropDownList<TValue,TEntity,TDataSource,TDataSourceId> : WorksDropDownListBase<TValue,TEntity,TDataSource,TDataSourceId>
    {   
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var sequence = 0;
            var type = TypeDropDown;

            var typeValue = typeof(TValue);
            var typeItem = typeof(TEntity);
           
            if (IsUseComboBoxItemDto)
            {
                typeValue = typeof(TDataSourceId);           
                typeItem = typeof(TDataSource);
            }

            Type[] typeArgs = { typeValue, typeItem };
            var typeDropDownComponent = typeof(SfComboBox<,>);          
            typeDropDownComponent = typeDropDownComponent.MakeGenericType(typeArgs).New().GetType();

            var typeDropDownEvent = typeof(ComboBoxEvents<,>);
            typeDropDownEvent = typeDropDownEvent.MakeGenericType(typeArgs).New().GetType();
            var typeDropDownFieldSettings = typeof(ComboBoxFieldSettings);
            object eventCallback;
            //var eventCallback1 = RuntimeHelpers.TypeCheck(EventCallback.Factory.Create<ChangeEventArgs<TDataSourceId>>(this, OnValueChanged));
            eventCallback = RuntimeHelpers.TypeCheck(CreateEvent<ChangeEventArgs<TDataSourceId,TDataSource>>(this, OnInputValueChanged));

            if (type == TypeDropDown.IsDropDownList)
            {
            
                typeDropDownComponent = typeof(SfDropDownList<,>);
                typeDropDownComponent = typeDropDownComponent.MakeGenericType(typeArgs).New().GetType();

                typeDropDownEvent = typeof(DropDownListEvents<,>);
                typeDropDownEvent = typeDropDownEvent.MakeGenericType(typeArgs).New().GetType();

                typeDropDownFieldSettings = typeof(DropDownListFieldSettings);
            }
            if (type == TypeDropDown.IsMultiSelect)
            {
                typeArgs = new Type[] { typeValue };
                typeDropDownComponent = typeof(SfMultiSelect<>);
                typeDropDownComponent = typeDropDownComponent.MakeGenericType(typeArgs).New().GetType();
                typeDropDownEvent = typeof(MultiSelectEvents<TValue>);
                typeDropDownFieldSettings = typeof(MultiSelectFieldSettings);

                eventCallback = RuntimeHelpers.TypeCheck(CreateEvent<MultiSelectChangeEventArgs<TValue>>(this, OnInputValueChanged));
            }


            builder.OpenComponent(sequence,typeDropDownComponent);
            var value = Value.ChangeType<TDataSourceId>();

            if  (type == TypeDropDown.IsComboBox)  builder.AddAttribute(++sequence, "Autofill", AutoFill);
            if (type == TypeDropDown.IsMultiSelect) builder.AddAttribute(++sequence, "Mode", this.VisualMode);

            builder.AddAttribute(++sequence, "DataSource", DataSource);
            builder.AddAttribute(++sequence, "Placeholder", Caption);
            builder.AddAttribute(++sequence, "Value", value);
            builder.AddAttribute(++sequence, "Locale", Locale);
            builder.AddAttribute(++sequence, "ShowClearButton", ShowClearButton);         
            builder.AddAttribute(++sequence, "AllowFiltering", AllowFilter);
            builder.AddAttribute(++sequence, "IgnoreAccent", IgnoreAccent);
            builder.AddAttribute(++sequence, "Enabled", Enabled);
            builder.AddAttribute(++sequence, "Readonly", !AllowEdit);
            builder.AddAttribute(++sequence, "CssClass", CssClass);
            builder.AddAttribute(++sequence, "FloatLabelType", FloatLabel);
            builder.AddAttribute(++sequence, "ChildContent", (RenderFragment)((settings) =>
            {
                var seq = 0;
                settings.OpenComponent(seq,typeDropDownEvent);
                if (!FilterBy.IsEmpty()) settings.AddAttribute(++seq, "Filtering", RuntimeHelpers.TypeCheck(EventCallback.Factory.Create<FilteringEventArgs>(this, OnValueFilter)));
                settings.AddAttribute(++seq, "ValueChange", eventCallback);
                settings.CloseComponent();

                seq = 0;
                settings.OpenComponent(seq, typeDropDownFieldSettings);
                settings.AddAttribute(++seq, "Text", FieldText);
                settings.AddAttribute(++seq, "Value", FieldValue);
                settings.CloseComponent();
            }));
            builder.AddComponentReferenceCapture(++sequence, (value) =>
            {
                InternalCmp = value;
            });

            builder.CloseComponent();

        }
        protected EventCallback<TEventCallback> CreateEvent<TEventCallback>(object receiver,Func<TEventCallback,Task> actionCallback)
        {
            var callback = EventCallback.Factory.Create<TEventCallback>(receiver, actionCallback);
            return callback;
        }

    }
   

}

