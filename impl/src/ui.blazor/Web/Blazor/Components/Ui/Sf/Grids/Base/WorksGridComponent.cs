using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Works.Web.Blazor.Components.Common;
using Works.Extensions;
using Works.Application.Services.Dto;
using Works.Domain.Entities;
using Works.Domain.Entities.Attributes;

namespace Works.Web.Blazor.Components.Ui.Sf.Grids
{
    public class  WorksGridComponent<TModel> : WorksWebBlazorComponent, IGridRefreshDataSource 
    {
        protected List<object> Tool { get; set; }
        protected IList<ColumnModelDto> GetColumnsFromModel;
        protected TModel CurrentModel { get; set; }
        [Parameter] public string ColumnFields { get; set; }
        [Parameter] public IList<ColumnModelDto> Columns { get; set; }
        [Parameter] public IEnumerable<TModel> DataSource { get; set; }
        [Parameter] public bool AllowFilter { get; set; } = false;
        [Parameter] public bool ButtonAddIsPrimaryKey { get; set; } = true;
        [Parameter] public bool AllowResize { get; set; } = false;
        [Parameter] public bool AllowGroup { get; set; } = false;
        [Parameter] public bool AllowPrint { get; set; } = false;
        [Parameter] public bool AllowPage { get; set; } = true;
        [Parameter] public bool AllowButtons { get; set; } = false;
        [Parameter] public bool AllowButtonReport { get; set; } = false;
        [Parameter] public bool AllowDisplayPrimaryKey { get; set; } = true;
        [Parameter] public bool AddToolbar { get; set; } = false;
        [Parameter] public string[] GroupColumns { get; set; }
        [Parameter] public EventCallback OnAdd { get; set; }
        [Parameter] public EventCallback<TModel> OnEdit { get; set; }
        [Parameter] public EventCallback<TModel> OnRemove { get; set; }
        [Parameter] public EventCallback<TModel> OnSelect { get; set; }
        [Parameter] public EventCallback OnPrint { get; set; }

        protected bool IsShouldRender = true;
        protected bool IsFirstRender = false;

        public virtual Task ReloadAsync(object data)
        {
            return Task.CompletedTask;
        }

      
        protected override void OnParametersSet()
        {
             
            if (AllowPrint)
            {
                Tool = (new List<object> { "Search", "Print" });                
            }

            if (AddToolbar)
            {
                Tool ??= new List<object> { "Add", "Edit","Delete" };     
            }
            GetColumnsFromModel = GetColumns();

        }
         
        protected async Task OnRowSelect(RowSelectEventArgs<TModel> args)
        {                                   
            try
            {
               
                CurrentModel = args.Data; 
                await OnSelect.InvokeAsync(CurrentModel);
                
            }
            catch (Exception ex)
            {
                ErrorMessages.Add($"Erro ao carregar dados da entidade {typeof(TModel)} <br> Erro {ex.Message}");
            }
        }

        private IList<ColumnModelDto> GetColumns()
        {
          //  if (!Columns.IsNull()) if (Columns.Any()) return Columns;
            var lstProperties = typeof(TModel).GetProperties();
            var lstFields = new List<PropertyInfo>();
            var lstColumns = new List<ColumnModelDto>();

            if (!ColumnFields.IsNullOrEmpty())
            {
                lstProperties = ColumnFields.Split(',')
                    .Where(s => lstProperties.Any(x => x.Name.Equals(s)))
                    .Select(x => lstProperties.SingleOrDefault(f => f.Name.Equals(x))).ToArray();

            }
            var _order = 100;
            foreach (var prop in lstProperties)
            {
                var pType = prop.PropertyType;
                var pName = prop.Name;

                var _field = new ColumnModelDto
                {
                    Field = prop.Name,
                    Caption = prop.Name,
                    Width = "150",
                    AllowResizing = false,
                    IsPrimaryKey = false,
                    AllowEditing = true,
                    Format = "",
                    TextAlign = TextAlign.Left,
                    Order = _order++
                    

                };
                if (pName.Equals("Item"))
                {

                    _field.AllowEditing = false;
                    _field.IsVisible = false;
                   
                }
                if (pName.Equals("Id"))
                {

                    _field.AllowEditing = false;                   
                    _field.IsPrimaryKey = true;
                    _field.Width = "30";
                    _field.IsVisible = AllowDisplayPrimaryKey;
                    if (ColumnFields.IsNull()) _field.Order = 1;
                }
                if (prop.Name.Equals("Nome"))
                {
                    //_field.Width = "250";
                    _field.AllowResizing = true;
                    if (ColumnFields.IsNull()) _field.Order = 2;
                }
                if (prop.PropertyType.IsCollection())
                {
                    _field.IsVisible = false;
                }
                var attr = prop.GetAttribute<Domain.Entities.Attributes.ModelFieldAttribute>();
                if (attr != null)
                {
                    if (attr.Order > 0) _field.Order = attr.Order;

                    _field.IsVisible = attr.IsVisible;
                  
                    _field.AllowResizing = attr.AllowResizing;
                    _field.AllowEditing = attr.AllowEditing;
                    _field.Caption = attr.Caption.IsNullOrEmpty() ? prop.Name : attr.Caption;
                    _field.Field = attr.FieldDisplay.IsNullOrEmpty() ? prop.Name : attr.FieldDisplay;
                }
                if (pType == typeof(bool))
                {
                    _field.Width = "60";
                    _field.Type = ColumnType.Boolean;
                    _field.IsCheckBox = true;
                }
                else if (pType == typeof(decimal?) || pType == typeof(decimal))
                {
                    _field.Width = "120";
                    _field.Format = "n2";
                    _field.Type = ColumnType.Number;
                    _field.TextAlign = TextAlign.Right;
                }
                else if (pType == typeof(int?) || pType == typeof(int))
                {
                    _field.Width = "60";
                    _field.Type = ColumnType.Number;
                }
                else if (pType == typeof(DateTime?) || pType == typeof(DateTime))
                {
                    _field.Width = "130";
                    _field.Format = "dd/MM/yyyy";
                    _field.Type = ColumnType.DateTime;
                }
                if (!AllowResize) _field.AllowResizing = false;
                lstColumns.Add(_field);
            }
            lstColumns = lstColumns.Where(f => f.IsVisible).ToList().OrderBy(f => f.Order).ToList();
            return lstColumns;
        }


        protected void ActionBegin(ActionEventArgs<TModel> args)
        {
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.BeginEdit)
            {
                // Triggers before editing operation starts
                OnEdit.InvokeAsync(CurrentModel);
            }
            else if (args.RequestType == Syncfusion.Blazor.Grids.Action.Add)
            {
                // Triggers before add operation starts
                OnAdd.InvokeAsync(null);

            }
            else if (args.RequestType == Syncfusion.Blazor.Grids.Action.Cancel)
            {
                // Triggers before cancel operation starts
            }
            else if (args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {
                // Triggers before save operation starts
            }
            else if (args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
            {
                // Triggers before delete operation starts
                OnRemove.InvokeAsync(CurrentModel);
            }
        }
    }
}
