using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Works.Application.Services;
using Works.Application.Services.Dto;
using Works.Expressions;

namespace Works.Web.Blazor.Components.Ui.Sf.Inputs
{
    public enum TypeDropDown
    {
        IsComboBox = 1,
        IsDropDownList=2,
        IsMultiSelect=3      

    }

    public abstract class SelectEventArgs<T>
    {
        public T Selected;
    }
    public class WorksComponentEvents<T> : ComponentBase
    {
        [Parameter]
        public EventCallback<SelectEventArgs<T>> OnSelected { get; set; }
    }

    

    public abstract class WorksDropDownListBase<TValue,TEntity,TDataSource,TDataSourceId> : WorksSfComponentBase<TValue>
    {
        protected TypeDropDown TypeDropDown { get; set; }
        protected bool IsUseComboBoxItemDto { get; set; }
        
        [Inject] protected IWorksLookUpService LookupService { get; set; }
        [Parameter] public VisualMode VisualMode { get; set; } = VisualMode.CheckBox;
        [Parameter] public bool AutoFill { get; set; } = true;
        [Parameter] public bool AllowFilter { get; set; } = true;
        [Parameter] public string FilterBy { get; set; }
        [Parameter] public Syncfusion.Blazor.DropDowns.FilterType FilterType { get; set; } = Syncfusion.Blazor.DropDowns.FilterType.StartsWith;
        [Parameter] public bool IgnoreAccent { get; set; } = true;
        [Parameter] public string FieldText { get; set; } = "FieldText";
        [Parameter] public string FieldValue { get; set; } = "FieldValue";
        [Parameter] public EventCallback<TValue> ValueChanged { get; set; }
        [Parameter] public EventCallback<TEntity> OnSelected { get; set;}
        [Parameter] public EventCallback<object> OnValueChanged { get; set; }
        [Parameter] public IEnumerable<TDataSource> DataSource { get; set; }
        [Parameter] public EventCallback<IEnumerable<TEntity>> OnMultiValueSelected { get; set; }
        [Parameter] public EventCallback<IEnumerable<TEntity>> OnMultiValueRemoved { get; set; }

        protected async Task InitializeDataSourceAsync()
        {
            if (DataSource == null)
            {
                if (typeof(TEntity).IsEnum)
                {
                    var dataSource = WorksEnum.ToComboBox<TEntity>(typeof(TEntity));
                    DataSource = dataSource as IEnumerable<TDataSource>;                 
                }
                else
                {
                    var dataSource = await LookupService.GetAllAsync<TEntity>();
                    DataSource = dataSource as IEnumerable<TDataSource>;
                }
                IsUseComboBoxItemDto = true;
            }
        }

        protected virtual async Task OnInputValueChanged(Syncfusion.Blazor.DropDowns.ChangeEventArgs<TDataSourceId,TDataSource> e)
        {
            var value = e.Value.ChangeType<TValue>();
            this.Value = value;
            var valueSelected = GetItemBy(value);
            if (ValueChanged.HasDelegate) await ValueChanged.InvokeAsync(value);
            if (OnSelected.HasDelegate) await OnSelected.InvokeAsync(valueSelected);           
            if (OnValueChanged.HasDelegate) await OnValueChanged.InvokeAsync(valueSelected);
            
        }
        #region MultiSelect
        protected virtual async Task OnInputValueChanged(MultiSelectChangeEventArgs<TValue> args)
        {
            var entityList = new List<TEntity>();
            var value = args.Value;

            if (ValueChanged.HasDelegate) await ValueChanged.InvokeAsync(value);
            if (args.OldValue != null)
            {

                var oldValues = args.OldValue as Array;
                if (oldValues != null)
                {
                    foreach (var item in oldValues)
                    {
                        var record = GetItemBy(item);
                        entityList.Add(record);
                    }
                }
                if (OnMultiValueRemoved.HasDelegate) await OnMultiValueRemoved.InvokeAsync(entityList.AsEnumerable<TEntity>());
            }
            if (args.Value != null)
            {
                entityList = new List<TEntity>();
                var newValues = args.Value as Array;
                if (newValues != null)
                {
                    foreach (var item in newValues)
                    {
                        var record = GetItemBy(item);
                        entityList.Add(record);
                    }
                }
                if (OnMultiValueSelected.HasDelegate) await OnMultiValueSelected.InvokeAsync(entityList.AsEnumerable<TEntity>());
            }

            StateHasChanged();

        }
        #endregion


        private TEntity GetItemBy(object value)
        {
            if (value == null)
            {
                var valueDefault = default(TEntity);
                if (!typeof(TEntity).IsEnum) valueDefault = (TEntity)typeof(TEntity).New();
                return valueDefault;                    
            };
            var pred = new WorksPredicateExpressionBuilder<TDataSource>();
            pred.Append(FieldValue, Operator.Equal, value);
            var where = pred.ToLambda();

            var item = DataSource.AsQueryable().SingleOrDefault(where);


            // var item = DataSource.SingleOrDefault(p => pi.GetValue(p).ChangeType(TDataSourceId).Equals(value));
            if (IsUseComboBoxItemDto)
            { 
                var itemDto = item as ComboBoxItemDto;
                if (itemDto != null) return itemDto.FieldData.ChangeType<TEntity>();
                else return default(TEntity);
            }
            return item.ChangeType<TEntity>();
         

            //var internalDataSource = DataSource as IEnumerable<object>;
            //var typeDataSource = IsUseComboBoxItemDto ? typeof(ComboBoxItemDto) : typeof(TItem);
            //var pi = typeDataSource.GetProperty(FieldValue);
            //var typeProperty = pi.PropertyType;

            //if (value == null) return default(TItem);
            //if (IsUseComboBoxItemDto)
            //{
            //    var valueSearch = value.ChangeType<int?>();
            //    var item = internalDataSource.SingleOrDefault(p => pi.GetValue(p).ChangeType<int?>().Equals(valueSearch));
            //    var itemDto = item as ComboBoxItemDto;
            //    if (itemDto != null) return itemDto.FieldData.ChangeType<TItem>();
            //    else return default(TItem);
            //}
            //else
            //{
            //    var item = internalDataSource.SingleOrDefault(p => pi.GetValue(p).ChangeType<TValue>().Equals(value));
            //    return item.ChangeType<TItem>();
            //}

        }

        protected async Task OnValueFilter(FilteringEventArgs args)
        {
            var internalControl = InternalCmp as SfDropDownList<TValue, TEntity>;
            var internalDataSource = DataSource as IEnumerable<TEntity>;

            args.PreventDefaultAction = true;
            var query = new Query().Where(new WhereFilter() { Field = FilterBy, Operator = "contains", value = args.Text, IgnoreCase = true });
            query = !string.IsNullOrEmpty(args.Text) ? query : new Query();
            await internalControl.Filter(internalDataSource, query);
        }
         
    }
  
}
