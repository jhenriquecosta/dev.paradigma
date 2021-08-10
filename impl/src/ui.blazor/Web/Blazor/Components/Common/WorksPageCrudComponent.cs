using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Components;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using Works.Web.Blazor.Ui.Sf.Modals;
using Works.Web.Enums;
using Works.Domain.Entities;
using Works.Application.Services.Dto;
using Works.Domain.Repositories;
using Works.Domain.Uow;
using Works.ObjectMapping;
using Works.Web.Blazor.Ui.Common;
using Works.Validations;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.TreeGrid;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components.CompilerServices;
using Blazorise;
using Microsoft.AspNetCore.Components.Rendering;
using Works.Web.Blazor.Ui.Sf.Inputs;
using System.Reflection;
using Works.Domain.Entities.Attributes;
using Works.Extensions;

namespace Works.Web.Blazor.Ui.Common
{

    public interface IPageDataModel<TPrimaryKey> : IWorksClassGeneric
    {
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>

        TPrimaryKey Id { get; set; }

        /// <summary>
        /// Checks if this entity is transient (not persisted to database and it has not an <see cref="Id"/>).
        /// </summary>
        /// <returns>True, if this entity is transient</returns>
        bool IsTransient();
    }


    public abstract class WorksPageCrudComponent<TModel, TModelDto> : WorksPageCrudComponent<TModel> where TModelDto : class, IEntityDto, new() where TModel : class, IEntity<int>, new()
    {

        protected new TModelDto PageModelInput { get; set; } = new TModelDto();
        protected IEnumerable<TModelDto> PageModelListDto { get; set; }
        protected override string GetTitle => typeof(TModelDto).Name;
        protected override string GetCaption => $"Registro Atual {PageModelInput?.Id}";
        protected override async Task OnInitializedAsync()
        {
            if (IsInitialized) return;
            await this.InitializeAsync();
            GridComponent = GetGrid<TModelDto>(PageModelListDto);
            GetPropertyInfo = typeof(TModelDto).GetProperties();
            IsInitialized = true;
        }

        protected async Task InitializeAsync()
        {
            PageModelListDto = await GetDataListDtoAsync();
        }
        protected async Task<IEnumerable<TModelDto>> GetDataListDtoAsync()
        {
            var modelList = await base.OnFindAllAsync();
            var dtoList = modelList.AsQueryable().MapTo<TModelDto>();
            return dtoList;
        }

        protected void Init(TModelDto model, WorksPageModelAction actionModel)
        {

            PageModelInput = GetEntity(model?.Id);
            PageModelAction = actionModel;
        }
        public override void OnPageModelAction(object valueModel, WorksPageModelAction actionModel)
        {
            TModelDto model = valueModel as TModelDto;
            this.IsVisible = true;
            this.Init(model, actionModel);
            if (actionModel != WorksPageModelAction.New && valueModel == null)
            {
                ExecuteAsync(() => InfoAsync("Selecione um registro!"));
                return;
            }
            this.OnModalShow(ModalFormMain);
        }

        protected override async Task OnPageRefreshAsync()
        {
            // await this.OnFindAllAsync();            
            await InitializeAsync();
            GridComponent = GetGrid<TModelDto>(PageModelListDto);
            OnModalHide(ModalFormMain);
            //OnChanged();
        }

        public new TModelDto GetEntity(int? idModel)
        {
            return PageModelListDto.SingleOrDefault(f => f.Id == idModel) ?? new TModelDto();
        }



        protected Task<bool> OnSaveOrUpdateAsync(TModelDto modelDtoSelected, bool question = true)
        {
            var entitySelected = modelDtoSelected.MapTo<TModel>();   // ObjectMapper.Map<TModel>(saveEntityDto);
            return base.OnSaveOrUpdateAsync(entitySelected);
        }
        protected Task<bool> OnDeleteAsync(TModelDto modelDtoSelected, bool question = true)
        {

            var entitySelected = modelDtoSelected.MapTo<TModel>();
            return base.OnDeleteAsync(entitySelected);
        }


    }





    public abstract class WorksPageCrudComponent<TEntity> : WorksPageCrudComponentBase<TSource, TSourceDto, TModel>
    where TSource : class, IEntity<int>, new()
    where TSourceDto : class, IEntityDto, new()
    where TModel : class, IEntity<int>, new()
    {





        public RenderFragment GetPanel<TSource>(object data) where TSource : class
        {
            return BuildPanel<TSource>(data);
        }
        public RenderFragment GetGrid<TSource>(object data) where TSource : class
        {
            return BuildGrid<TSource>(data);
        }








        //[UnitOfWork]











        #region Build Dynamic Form

        protected RenderFragment GetContainerComponent<TSource>(object data) where TSource : class
        {
            var sequence = 0;
            RenderFragment render = form =>
            {
                form.OpenComponent<WorksContainer>(sequence);
                form.AddAttribute(++sequence, "IsInitialized", IsInitialized);
                form.AddAttribute(++sequence, "ChildContent", BuildPanel<TSource>(data));
                form.CloseComponent();
            };

            return render;
        }
        protected RenderFragment BuildPanel<TSource>(object data) where TSource : class
        {
            var sequence = 0;
            RenderFragment render = builder =>
            {
                builder.OpenComponent<WorksPanel>(sequence);
                builder.AddAttribute(++sequence, "Title", GetTitle);
                builder.AddAttribute(++sequence, "SubTitle", "Listagem");
                builder.AddAttribute(++sequence, "ChildContent", BuildGrid<TSource>(data));
                builder.CloseComponent();
            };
            return render;

        }

        protected RenderFragment BuildGrid<TSource>(object data) where TSource : class
        {
            var grid_sequence = 0;
            var workGrid = WorksReflection.CreateInstance<WorksGrid<TSource>>();

            RenderFragment render = grid =>
            {
                grid.OpenComponent<WorksGrid<TSource>>(++grid_sequence);
                grid.AddAttribute(++grid_sequence, "AllowButtons", RuntimeHelpers.TypeCheck<bool>(true));
                grid.AddAttribute(++grid_sequence, "AllowPrint", RuntimeHelpers.TypeCheck<bool>(true));
                grid.AddAttribute(++grid_sequence, "DataSource", data);
                grid.AddAttribute(++grid_sequence, "OnAdd", RuntimeHelpers.TypeCheck(EventCallback.Factory.Create(this, (value) => OnPageModelAction(null, WorksPageModelAction.New))));
                grid.AddAttribute(++grid_sequence, "OnEdit", RuntimeHelpers.TypeCheck(EventCallback.Factory.Create<TSource>(this, (value) => OnPageModelAction(value, WorksPageModelAction.Edit))));
                grid.AddAttribute(++grid_sequence, "OnRemove", RuntimeHelpers.TypeCheck(EventCallback.Factory.Create<TSource>(this, (value) => OnPageModelAction(value, WorksPageModelAction.Delete))));
                grid.CloseComponent();
            };
            return render;
        }
        protected RenderFragment EditFormComponent()
        {
            if (!IsInitialized) return null;
            var sequence = 0;

            RenderFragment render = builder =>
            {
                builder.OpenComponent<WorksModalForm>(sequence);
                builder.AddAttribute(++sequence, "Caption", GetCaption);
                builder.AddAttribute(++sequence, "OnDelete", RuntimeHelpers.TypeCheck(EventCallback.Factory.Create(this, () => OnDeleteAsync(this.PageModelInput))));
                builder.AddAttribute(++sequence, "OnSave", RuntimeHelpers.TypeCheck(EventCallback.Factory.Create(this, () => OnSaveOrUpdateAsync(this.PageModelInput))));
                builder.AddAttribute(++sequence, "BodyContent", BuildFields());
                builder.AddComponentReferenceCapture(++sequence, (value) =>
                {
                    ModalFormMain = value as WorksModalForm;
                });
                builder.CloseComponent();
            };
            return render;

        }
        protected RenderFragment BuildFields()
        {
            var fields_sequence = 0;
            var lookup = GetColumns().ToLookup(f => f.Fieldset);

            RenderFragment renderFields = builder =>
            {
                foreach (var key in lookup)
                {
                    var typeField = key == null ? typeof(Field) : typeof(Fields);
                    builder.OpenComponent(fields_sequence, typeField);
                    builder.AddAttribute(++fields_sequence, "ChildContent", (RenderFragment)((field) =>
                    {
                        var sequence = 0;
                        foreach (var column in key)
                        {
                            var controlRender = AddTextField(column);
                            var containerType = key.Key == null ? typeof(FieldBody) : typeof(Field);
                            field.OpenComponent(sequence, containerType);
                            if (key.Key != null) field.AddAttribute(++sequence, "ColumnSize", column.FieldSize);
                            field.AddAttribute(++sequence, "ChildContent", controlRender);
                            field.CloseComponent();
                        }

                    }));

                    builder.CloseComponent();
                }
            };
            return renderFields;
        }
        protected RenderFragment AddField(ColumnModelDto column)
        {

            var sequence = 0;
            RenderFragment renderField = field =>
            {

                field.OpenComponent<Field>(sequence);
                field.AddAttribute(++sequence, "ChildContent", AddTextField(column));
                field.CloseComponent();
            };
            return renderField;
        }
        protected RenderFragment AddTextField(ColumnModelDto column)
        {

            var sequence = 0;
            RenderFragment textField = textEdit =>
            {
                var value = PageModelInput[column.Field];

                if (column.FieldType == typeof(int) || column.FieldType == typeof(int?))
                {
                    textEdit.OpenComponent<WorksInput<int?>>(sequence);
                    textEdit.AddAttribute(++sequence, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<int?>>(EventCallback.Factory.Create(this, RuntimeHelpers.CreateInferredEventCallback(this, __value => PageModelInput[column.Field] = __value, PageModelInput[column.Field].ChangeType<int?>()))));
                }
                else if (column.FieldType == typeof(decimal) || column.FieldType == typeof(decimal?))
                {
                    textEdit.OpenComponent<WorksInput<decimal?>>(sequence);
                    textEdit.AddAttribute(++sequence, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<decimal?>>(EventCallback.Factory.Create(this, RuntimeHelpers.CreateInferredEventCallback(this, __value => PageModelInput[column.Field] = __value, PageModelInput[column.Field].ChangeType<decimal?>()))));
                }
                else if (column.FieldType.IsDate())
                {
                    textEdit.OpenComponent<WorksInput<DateTime?>>(sequence);
                    textEdit.AddAttribute(++sequence, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<DateTime?>>(EventCallback.Factory.Create(this, RuntimeHelpers.CreateInferredEventCallback(this, __value => PageModelInput[column.Field] = __value, PageModelInput[column.Field].ChangeType<DateTime?>()))));
                }
                else if (column.FieldType.IsBoolean())
                {
                    textEdit.OpenComponent<WorksInput<bool>>(sequence);
                    textEdit.AddAttribute(++sequence, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<bool>>(EventCallback.Factory.Create(this, RuntimeHelpers.CreateInferredEventCallback(this, __value => PageModelInput[column.Field] = __value, PageModelInput[column.Field].ChangeType<bool>()))));
                }
                //entidade or enum (comboBox)
                else if (column.FieldType.IsEntity() || column.FieldType.IsEnum)
                {
                    var valueForConvert = value;
                    if (column.FieldType.IsEntity())
                    {
                        if (value == null)
                        {
                            value = column.FieldType.New();
                        }
                        valueForConvert = value;
                        value = value.GetPropertyValue(value.GetType(), "Id");
                        if (value.ToString().Equals("0")) value = "";
                    }


                    var typeCombo = typeof(WorksComboBox<>);
                    typeCombo = typeCombo.MakeGenericType(new Type[] { column.FieldType }).New().GetType();
                    textEdit.OpenComponent(sequence, typeCombo);
                    textEdit.AddAttribute(++sequence, "GetDataSelected", RuntimeHelpers.TypeCheck<EventCallback<object>>(EventCallback.Factory.Create(this, RuntimeHelpers.CreateInferredEventCallback(this, __value => PageModelInput[column.Field] = __value.ChangeType(valueForConvert), PageModelInput[column.Field]))));

                }
                else
                {
                    textEdit.OpenComponent<WorksInput<string>>(sequence);
                    textEdit.AddAttribute(++sequence, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<string>>(EventCallback.Factory.Create(this, RuntimeHelpers.CreateInferredEventCallback(this, __value => PageModelInput[column.Field] = __value, PageModelInput[column.Field].ChangeType<string>()))));

                }
                textEdit.AddAttribute(++sequence, "Value", value);
                textEdit.AddAttribute(++sequence, "Caption", column.Caption);
                textEdit.CloseComponent();
            };
            return textField;
        }
        protected IFluentColumn GetFluentColumn(FieldSizeValue value)
        {
            switch (value)
            {
                case FieldSizeValue.IsAuto: return ColumnSize.IsAuto.OnDesktop.IsAuto.OnMobile;
                case FieldSizeValue.Is1: return ColumnSize.Is1.OnDesktop.Is1.OnMobile;
                case FieldSizeValue.Is2: return ColumnSize.Is2.OnDesktop.Is2.OnMobile;
                case FieldSizeValue.Is3: return ColumnSize.Is3.OnDesktop.Is3.OnMobile;
                case FieldSizeValue.Is4: return ColumnSize.Is4.OnDesktop.Is4.OnMobile;
                case FieldSizeValue.Is5: return ColumnSize.Is5.OnDesktop.Is5.OnMobile;
                case FieldSizeValue.Is6: return ColumnSize.Is6.OnDesktop.Is6.OnMobile;
                case FieldSizeValue.Is7: return ColumnSize.Is7.OnDesktop.Is7.OnMobile;
                case FieldSizeValue.Is8: return ColumnSize.Is8.OnDesktop.Is8.OnMobile;
                case FieldSizeValue.Is9: return ColumnSize.Is9.OnDesktop.Is9.OnMobile;
                case FieldSizeValue.Is10: return ColumnSize.Is10.OnDesktop.Is10.OnMobile;
                case FieldSizeValue.Is11: return ColumnSize.Is11.OnDesktop.Is11.OnMobile;
                case FieldSizeValue.Is12: return ColumnSize.Is12.OnDesktop.Is12.OnMobile;
                case FieldSizeValue.IsFull: return ColumnSize.IsFull.OnDesktop.IsFull.OnMobile;
                case FieldSizeValue.IsHalf: return ColumnSize.IsHalf.OnDesktop.IsHalf.OnMobile;
                case FieldSizeValue.IsThird: return ColumnSize.IsThird.OnDesktop.IsThird.OnMobile;
                case FieldSizeValue.IsQuarter: return ColumnSize.IsQuarter.OnDesktop.IsQuarter.OnMobile;


                default: return ColumnSize.IsAuto.OnDesktop.IsAuto.OnMobile;
            }
        }
        private IList<ColumnModelDto> GetColumns()
        {

            var lstProperties = GetPropertyInfo;
            var lstFields = new List<PropertyInfo>();
            var lstColumns = new List<ColumnModelDto>();

            var _order = 100;
            foreach (var prop in lstProperties)
            {
                var pType = prop.PropertyType;
                var pName = prop.Name;

                var _field = new ColumnModelDto
                {
                    FieldType = prop.PropertyType,
                    Field = prop.Name,
                    Caption = prop.Name,
                    Width = "150",
                    AllowResizing = false,
                    IsPrimaryKey = false,
                    AllowEditing = true,
                    Format = "",
                    TextAlign = TextAlign.Left,
                    FieldSize = ColumnSize.IsAuto,
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
                    _field.IsVisible = false;
                    _field.Order = 1;
                }
                if (prop.Name.Equals("Nome"))
                {
                    //_field.Width = "250";
                    _field.AllowResizing = true;
                    _field.Order = 2;
                }
                if (prop.PropertyType.IsCollection())
                {
                    _field.IsVisible = false;
                }
                var attr = prop.GetAttribute<ModelFieldAttribute>();
                if (attr != null)
                {
                    if (attr.Order > 0) _field.Order = attr.Order;

                    _field.IsVisible = attr.IsVisible;
                    _field.Fieldset = attr.Fieldset;
                    _field.AllowResizing = attr.AllowResizing;
                    _field.AllowEditing = attr.AllowEditing;
                    _field.Caption = attr.Caption.IsNullOrEmpty() ? prop.Name : attr.Caption;
                    _field.Field = attr.FieldDisplay.IsNullOrEmpty() ? prop.Name : attr.FieldDisplay;
                    _field.FieldSize = GetFluentColumn(attr.FieldSize);

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
                lstColumns.Add(_field);
            }
            lstColumns = lstColumns.Where(f => f.IsVisible).ToList().OrderBy(f => f.Order).ToList();
            return lstColumns;
        }

    }

    #endregion
    //public static class RenderFragmentExtensions
    //{
    //    public static void AddHtml(this RenderTreeBuilder renderTreeBuilder,int renderSequence, string template)
    //    {
    //        var sequence = 0;
    //        RenderFragment render = builder =>
    //        {

    //            builder.OpenElement(sequence, "div");
    //            builder.AddContent (++sequence,template);
    //            builder.CloseElement();              
    //        };
    //        renderTreeBuilder.AddAttribute(++sequence, "ChildContent", render);
    //        //return render;
    //    }
    //    public static void AddField(this RenderTreeBuilder render,int sequence, object template)
    //    {
    //        render.OpenComponent<Field>(++sequence);
    //        render.AddAttribute(++sequence, "ChildContent",template);
    //        render.CloseComponent();
    //    }
    //    public static void AddFields(this RenderTreeBuilder render, int sequence, object template)
    //    {
    //        render.OpenComponent<Fields>(++sequence);
    //        render.AddAttribute(++sequence, "ChildContent", template);
    //        render.CloseComponent();
    //    }
    //    public static void AddFieldText(this RenderTreeBuilder render, int sequence, object template)
    //    {
    //        render.OpenComponent<WorksFieldText>(++sequence);            
    //        render.CloseComponent();
    //    }
    //}

}
