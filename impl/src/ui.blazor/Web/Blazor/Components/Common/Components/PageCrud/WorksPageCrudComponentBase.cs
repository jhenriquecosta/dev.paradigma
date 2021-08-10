using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Components;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using Works.Web.Blazor.Components.Ui.Sf.Modals;
using Works.Web.Enums;
using Works.Domain.Entities;
using Works.Application.Services.Dto;
using Works.Domain.Repositories;
using Works.Domain.Uow;
using Works.ObjectMapping;
using Works.Web.Blazor.Components.Common;
using Works.Validations;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.TreeGrid;
using JetBrains.Annotations;
using Works.Web.Blazor.Components.Ui.Alert;
using Works.Web.Blazor.Components.Ui.LoadIndicator;
using Works.Application.Services;
using Works.Web.Blazor.Components.Ui.Sf.Forms;
using Works.Web.Blazor.Components.Ui.Sf.Panels;
using Works.Web.Blazor.Components.Ui.Sf.Grids;
using Microsoft.AspNetCore.Components.CompilerServices;
using Blazorise;
using Microsoft.AspNetCore.Components.Rendering;
using Works.Web.Blazor.Components.Ui.Sf.Inputs;
using System.Reflection;
using Works.Domain.Entities.Attributes;
using Works.Extensions;
 
namespace Works.Web.Blazor.Components.Common
{
    

    public abstract class WorksPageCrudComponentBase<TEntity> : WorksPageCrudComponentBase<TEntity, TEntity>
    where TEntity : class, IEntity<int>, new()
    {
         
    }

    public abstract class WorksPageCrudComponentBase<TEntity, TModel> : WorksBaseComponent where TEntity : class, IEntity<int>, new() where TModel : class, IWorksClassGeneric<int>, new()
    {
        [Inject] protected IWorksLookUpService LookUpService { get; set; }
        [Inject] protected WorksPageDataTransfer WorksPageDataTransfer { get; set; }
        [Inject] protected IUnitOfWorkManager UnitOfWorkManager { get; set; }
        [Inject] protected IRepository<TEntity> DataContext { get; set; }
        [Inject] protected WorksComponentsService WorksCmpService { get; set; }
        [Parameter] public string Title { get; set; } = typeof(TModel).Name;
        [Parameter] public string SubTitle { get; set; } = "Adicionar/Alterar/Consultar/Excluir";
        [Parameter] public Expression<Func<TEntity, bool>> Filter { get; set; } = null;
        [Parameter] public string IndicatorContext { get; set; } = "pageinitcontext";

        protected WorksGrid<TModel> DataListGrid { get; set; }
        protected WorksModalForm ModalFormMain { get; set; }
        protected WorksPageModelAction PageModelAction { get; set; } = WorksPageModelAction.New;
        protected TModel PageModelInput { get; set; } = new TModel();
        protected IEnumerable<TModel> PageModelList { get; set; }
       
        protected RenderFragment DataPageCrudComponent { get; set; }
        protected bool HasRender { get; set; } = true;
        protected PropertyInfo[] GetPropertyInfo { get; set; }
        
        protected override async Task OnInitializedAsync()
        {
            if (IsInitialized) return;
            await this.InitializeAsync();          
            IsInitialized = true;
        }
        protected override bool ShouldRender()
        {
            if (ModalFormMain != null)
            {
                HasRender = !ModalFormMain.IsVisible;
            }
            return HasRender;
        }
      
        protected virtual string GetCaption(string msg = default)
        {
            if (msg.IsEmpty())
            {     
                msg = $"Registro Atual {PageModelInput?.Id}";
            }
            return msg;
        }
        private async Task InitializeAsync()
        {
            await WorksCmpService.LoadingTaskService.StartTaskAsync(async (task) =>
            {
                OnLoadData(await OnFindAllAsync());
                GetPropertyInfo = typeof(TModel).GetProperties();
                DataPageCrudComponent = GetContainerComponent();
                await Task.Delay(5);
            }, IndicatorContext, "requisitando registros...", "Aguarde..");
        }

        public virtual void Init(TModel model, WorksPageModelAction actionModel)
        {
             
            PageModelInput = GetEntity(model?.Id);
            PageModelAction = actionModel;
        }
        public virtual void OnPageModelAction(object valueModel, WorksPageModelAction actionModel)
        {
            TModel model = valueModel as TModel;
            this.IsVisible = true;
            this.Init(model, actionModel);
            if (actionModel != WorksPageModelAction.New && valueModel == null)
            {
                WorksCmpService.ExecuteAsync(() => WorksCmpService.InfoAsync("Selecione um registro!"));
                return;
            }

            this.OnModalShow(ModalFormMain);
        }

        public virtual TModel GetEntity(int? value)
        {
            var pi = typeof(TModel).GetProperty("Id");
            return PageModelList.SingleOrDefault(p => p.Id == value) ?? new TModel(); /* PageModelList.SingleOrDefault(p => pi.GetValue(p).ChangeType<int>().Equals(value)) ?? new TModel();*/
        }

        private bool IsEntity()
        {
            if (typeof(TModel).IsImplementing(typeof(IEntityDto))) return false;
            return true;
        }
        private IEnumerable<TModel> OnLoadData(IEnumerable<TEntity> datasource)
        {
            if (typeof(TModel).IsImplementing(typeof(IEntityDto)))
            {
                PageModelList = datasource.AsQueryable().MapTo<TModel>();
            }
            else PageModelList = datasource as IEnumerable<TModel>;
            return PageModelList;
        }
       
        protected virtual async Task OnPageRefreshAsync()
        {
            var dataSource = OnLoadData(await OnFindAllAsync());
            await DataListGrid.ReloadAsync(dataSource);
            OnModalHide(ModalFormMain);
           // ShouldRenderize();
        }
        #region modal
        protected void OnModalHide(WorksModalForm workModal)
        {
            if (workModal == null) return;
            if (workModal.IsVisible) workModal.Hide();
            
        }
        protected void OnModalShow(WorksModalForm workModal)
        {
            //ShouldRenderize();
            if (workModal == null) return;
            workModal?.SetCaption(GetCaption());
            workModal?.Show(PageModelAction);

        }
        #endregion
        #region crud
        public async Task<IEnumerable<TEntity>> OnFindAllAsync()
        {
            var resultList = new List<TEntity>();
            var uow = UnitOfWorkManager.Begin();
            resultList = Filter == null ? await DataContext.GetAllListAsync() : await DataContext.GetAllListAsync(Filter);
            await uow.CompleteAsync();            
            return resultList;
        }
        private TEntity ToEntity(TModel modelSelected)
        {
            TEntity entitySelected = null;
            if (modelSelected is IEntityDto)
            {
                entitySelected = modelSelected.MapTo<TEntity>();
            }
            else entitySelected = modelSelected as TEntity;
            return entitySelected;
        }
        protected virtual async Task<bool> OnSaveOrUpdateAsync(TModel modelSelected, bool question = true)
        {

            var entitySelected = ToEntity(modelSelected);
            if (!await Validate(entitySelected)) return false;
            if (question) if (!await WorksCmpService.QuestionAsync($"Salvar as alterações para o registro:  {entitySelected} ?")) return false;

            try
            {
                await DataContext.FlushChangesAsync();                
                await DataContext.InsertOrMergeAsync(entitySelected);


            }
            catch (Exception ex)
            {
                await WorksCmpService.ErrorAsync($"Erro {ex.Message}");
                return false;
            }

            WorksCmpService.Toast.ShowSuccess("Registro GRAVADO!", "Gravando...");

            await OnPageRefreshAsync();
            return true;
        }
        protected virtual async Task<bool> OnDeleteAsync(TModel modelSelected)
        {

            var entitySelected = ToEntity(modelSelected);
            if (!await WorksCmpService.QuestionAsync($"Deletar o registro {entitySelected.Id}-{entitySelected}?")) return false;
            if (entitySelected.IsTransient())
            {
                WorksCmpService.Toast.ShowInfo("Registro não localizado!", "Deletando...");
                return false;
            }
            try
            { 
                await DataContext.EvictAsync(entitySelected);
                await DataContext.DeleteAsync(entitySelected);
            }
            catch (Exception ex)
            {
                var erro = $"O seguinte erro ocorreu ao DELETAR o registro solicitado:<br> <b>{ex.Message}</b>";
                await WorksCmpService.ErrorAsync(erro);
                return false;
            }
            WorksCmpService.Toast.ShowInfo("registro deletado...", "Deletando...");
            //WorksCmpService.ShowAlert("Registro Deletado", AlertMode.Warning);
            await OnPageRefreshAsync();
            return true;
        }

        protected async Task<bool> Validate(object objectValidation)
        {
            var errorList = new StringBuilder();
            if (objectValidation is Validations.IValidation modelValidation)
            {
                var errorMessages = modelValidation.Validate();
                if (errorMessages.Count <= 0) return true;
                foreach (var result in errorMessages)
                {
                    var item = $"Campo: <b>{result.MemberNames.First().ToString()}</b> Erro: <b>{result.ErrorMessage}</b></br>";
                    errorList.AppendLine(item);
                }
            }

            await WorksCmpService.ErrorAsync(errorList.ToString());
            return false;
        }
        #endregion

        #region Build Dynamic Form

        protected RenderFragment GetContainerComponent()
        {
            var sequence = 0;
            RenderFragment render = form =>
            {
                form.OpenComponent<WorksContainer>(sequence);
                form.AddAttribute(++sequence, "IsInitialized", IsInitialized);
                form.AddAttribute(++sequence, "UseIndicator", false);
                form.AddAttribute(++sequence, "ChildContent", BuildPanel());
                form.CloseComponent();
            };

            return render;
        }
        protected RenderFragment BuildPanel()
        {
            var sequence = 0;
            RenderFragment render = builder =>
            {
                builder.OpenComponent<WorksPanel>(sequence);
                builder.AddAttribute(++sequence, "Title", this.Title);
                builder.AddAttribute(++sequence, "SubTitle", this.SubTitle);
                builder.AddAttribute(++sequence, "ChildContent", BuildGrid());
                builder.CloseComponent();
            };
            return render;

        }

        protected RenderFragment BuildGrid()
        {
            var grid_sequence = 0;
           // var workGrid = WorksReflection.CreateInstance<WorksGrid<TModel>>();

            RenderFragment render = grid =>
            {
                grid.OpenComponent<WorksGrid<TModel>>(++grid_sequence);
                grid.AddAttribute(++grid_sequence, "AllowButtons", RuntimeHelpers.TypeCheck<bool>(true));
                grid.AddAttribute(++grid_sequence, "AllowPrint", RuntimeHelpers.TypeCheck<bool>(true));
                grid.AddAttribute(++grid_sequence, "DataSource", PageModelList);
                grid.AddAttribute(++grid_sequence, "OnAdd", RuntimeHelpers.TypeCheck(EventCallback.Factory.Create(this, (value) => OnPageModelAction(null, WorksPageModelAction.New))));
                grid.AddAttribute(++grid_sequence, "OnEdit", RuntimeHelpers.TypeCheck(EventCallback.Factory.Create<TModel>(this, (value) => OnPageModelAction(value, WorksPageModelAction.Edit))));
                grid.AddAttribute(++grid_sequence, "OnRemove", RuntimeHelpers.TypeCheck(EventCallback.Factory.Create<TModel>(this, (value) => OnPageModelAction(value, WorksPageModelAction.Delete))));
                grid.AddComponentReferenceCapture(++grid_sequence, (value) =>
                {
                    DataListGrid = value as WorksGrid<TModel>;
                });
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
                builder.AddAttribute(++sequence, "Caption", GetCaption());
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
                    //var typeField = key == null ? typeof(Field) : typeof(Fields);
                    var typeField = typeof(Fields);
                    builder.OpenComponent(fields_sequence, typeField);
                    builder.AddAttribute(++fields_sequence, "ChildContent", (RenderFragment)((field) =>
                    {
                        var sequence = 0;
                        foreach (var column in key)
                        {
                            var fieldSize = GetFluentColumn(FieldSizeValue.IsFull);
                            if (key.Key != null) fieldSize = column.FieldSize;

                            var controlRender = AddTextField(column);                           
                            var containerType = typeof(Field);
                            field.OpenComponent(++sequence, containerType);                           
                            field.AddAttribute(++sequence, "ColumnSize", fieldSize);
                            field.AddAttribute(++sequence, "ChildContent", controlRender);
                            field.CloseComponent();
                            //field.AddMarkupContent(++sequence, "\r\n");
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
            RenderFragment outputFragment = null;

            try
            {
                var sequence = 0;
                RenderFragment textField = textEdit =>
                {
                    var fieldName = column.Field.Split('.')[0];

                    var value = PageModelInput[fieldName];

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
                else if (column.FieldType.IsEntity() || column.FieldType.IsEntityDto() || column.FieldType.IsEnum)
                    {
                        var valueForConvert = value;
                        var valueTypeSource = column.FieldTypeSource ?? column.FieldType;
                        var valueType = column.FieldType;
                        if (valueType.IsEntity() || valueType.IsEntityDto())
                        {
                            if (value == null)
                            {
                                value = valueType.New();
                            }
                            valueForConvert = value;
                            value = value.GetPropertyValue(value.GetType(), "Id");
                            if (value.ToString().Equals("0")) value = "";
                        }


                        var typeCombo = typeof(WorksComboBox<>);
                        typeCombo = typeCombo.MakeGenericType(new Type[] { valueTypeSource }).New().GetType();
                        textEdit.OpenComponent(sequence, typeCombo);
                        textEdit.AddAttribute(++sequence, "OnValueChanged", RuntimeHelpers.TypeCheck<EventCallback<object>>(EventCallback.Factory.Create(this, RuntimeHelpers.CreateInferredEventCallback(this, __value => PageModelInput[fieldName] = ConvertValue(__value, valueForConvert), PageModelInput[fieldName]))));

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
                outputFragment = textField;
            }
            catch (Exception ex)
            {

                var error = $"Erro {ex.Message} {Environment.NewLine} Campo: {column.Field}";
                outputFragment = AddFieldError(error);

            }
            return outputFragment;
        }
        protected RenderFragment AddFieldError(string err)
        {
            
            RenderFragment renderField = field =>
            {
                field.OpenElement(1, "span");
                field.AddContent(2,err);
                field.CloseComponent();
            };
            return renderField;
        }
        protected TReturn ConvertValue<TValue,TReturn>(TValue value, TReturn valueTypeDefault)
        {
           
            if (value == null) return default(TReturn);
            if (value.GetType() == valueTypeDefault.GetType()) return value.ChangeType(valueTypeDefault);
            var changeType = value.GetType().New();
            var resultValue = value.ChangeType(changeType);
            var valueDto = resultValue.MapTo(valueTypeDefault);
            return valueDto;
            
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
                    _field.FieldTypeSource = attr.FieldTypeSource;

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
}

