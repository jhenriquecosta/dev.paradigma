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

namespace Works.Web.Blazor.Components.Ui.Sf.Forms
{
    public class TreeDataDto
    {
        public int Id { get; set; }
        public string IdInterno { get; set; }
        public int? Ancestral { get; set; }
        public string Display { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public int Level { get; set; }
        public int Ordem { get; set; }

    }

    public abstract class WorksTreePageModel<TModel, TModelDto> : WorksPageModel<TModel> where TModelDto : class, IEntityDto, new() where TModel : class, ITreeEntity, IEntity<int>, new()
    {
        protected SfTreeGrid<TreeDataDto> WorksTreeGrid { get; set; }
        protected TModelDto TreeViewModel { get; set; } = new TModelDto();
        protected TModelDto AncestralViewModel { get; set; } = new TModelDto();
        protected List<TModelDto> WorksPageModelEntityDtoList { get; set; } = new List<TModelDto>();
        protected List<TreeDataDto> DataSourceList { get;set;} = new List<TreeDataDto>();
        protected TreeDataDto TreeDataDtoSelected { get; set; } = new TreeDataDto();

        protected override async Task OnInitializedAsync()
        {
            //await base.OnInitializedAsync();
            if (IsInitialized) return;
            await this.OnFindAllAsync();
            IsInitialized = true;
           // await this.InitializeComponentsAsync();
        }
        protected async Task InitializeComponentsAsync()
        {   
            await StartTaskAsync(() =>
            {
                return Task.Run(() =>
                {  
                    DataSourceList = OnGetAllAsync();
                });
            });
         
        }
        protected List<TreeDataDto> OnGetAllAsync(int? configId = null)
        {

            var result = new List<TreeDataDto>();
            if (WorksPageModelEntities != null)
            {
                var entitiesList = WorksPageModelEntities;
              //  if (configId != null && configId > 0) entitiesList = WorksPageModelEntities.Where(f => f.Id == configId || f.Config.Key == configId);
                result = entitiesList.Select(f => new TreeDataDto()
                {
                    Id = f.Id,
                    Ancestral = f.Ancestral,
                    Display = $"{f.Nome}",                 
                    IdInterno = f.IdInterno,
                    Level = f.Level,
                    Path = f.Path
                }).OrderBy(f => f.Id).ThenBy(f => f.Ancestral).ToList();
            }        
            return result;
        }

        protected IEnumerable<TModelDto> OnGetAllDtoList()
        {
            var result = base.OnGetAllList();
            return result?.AsQueryable().MapTo<TModelDto>();
        }
        protected void OnGetSelectedRow(RowSelectEventArgs<TreeDataDto> args)
        {
            try
            {
                TreeDataDtoSelected = args.Data;
            }
            catch (Exception ex)
            {
                ShowAlert($"Erro ao carregar dados da entidade {typeof(TreeDataDto)} <br> Erro {ex.Message}", AlertMode.Error);
            }
        }
        protected async Task<bool> OnSaveOrUpdateAsync(TModelDto saveEntityDto, bool question = true)
        {
            var entityToSave = ObjectMapper.Map<TModel>(saveEntityDto);
            var result = await base.OnSaveOrUpdateAsync(entityToSave);
            if (result) await this.InitializeComponentsAsync();
            return result;

        }
        protected Task<bool> OnDeleteAsync(TModelDto deleteEntityDto, bool question = true)
        {
            var deleteEntity = ObjectMapper.Map<TModel>(deleteEntityDto);
            return base.OnDeleteAsync(deleteEntity);
        }

        public async Task<TModelDto> AddItemAsync(int? parent, Expression<Func<TModel, bool>> filter, string format = null)
        {
            var getChildrensAsync = await GetChildrensAsync(filter);
            var totalChildrens = getChildrensAsync.ToList().Count();

            var _level = 0;
            var _parents = string.Empty;
            var _count = await DataContext.CountAsync(filter);
            _count++;
            var _path = $"{_count}";
            if (parent != null)
            {
                var _parent = await DataContext.SingleAsync(f => f.Id == parent);
                _parents = _parent.Parents;
                _parents = _parents.IsNullOrEmpty() ? $"{_parent.Id}" : $"{_parent.Parents};{_parent.Id}";
                _level = _parent.Level + 1;
                _path = _parent.Level == 0 ? $"{_count}" : $"{_parent.Path}.{_count}";
            }


            _path = ApplyDataFormat(_level, _count, _path, format);
            var _ordem = _path.RemoveNonAlphanumericChar().ChangeType<int>();
            var _multi = 1000;
            if (_ordem >= 10) { _multi = 100; }
            if (_ordem >= 100) { _multi = 10; }
            _ordem = _ordem * _multi;
            var ordem = _ordem.ToString("000000");

            if (_level == 0) _ordem = 0;
            var record = new TModel();
            record.InitPath(_path, _level);
            record.Parents = _parents;
            record.Ancestral = parent;
            record.Ordem = ordem;
            return record.MapTo<TModelDto>();
        }
      
        private  string ApplyDataFormat(int level, int value, string path, string format)
        {
            if (level == 0)
            {
                return $"{value:0000}"; ;
            }

            if (format.IsNullOrWhiteSpace())
            {
                return path;
            }

            var mask = path.Split('.');
            var x = mask;
            //diminui o nivel para ajustar o tamanho do preenchimento.
            //  level--;

            for (int i = 0; i < mask.Length; i++)
            {
                if (i == level)
                {
                    mask[i] = value.ToString(mask[i]);
                }

            }
            return string.Join(".", mask);
        }
        public async Task<int> CounterChildrensAsync(int ancestralId)
        {
            Expression<Func<TModel, bool>> filter = f => f.Ancestral == ancestralId;
            return await DataContext.CountAsync(filter);
        }
        public async Task<IList<TModel>> GetChildrensAsync([CanBeNull] Expression<Func<TModel, bool>> filter)
        {
            return await DataContext.GetAllListAsync(filter);
        }

    }

    public abstract class WorksPageModel<TModel, TModelDto> : WorksPageModel<TModel> where TModelDto : class, IEntityDto, new() where TModel : class, IEntity<int>, new()
    {
        protected override IGridRefreshDataSource WorksPageModelGrid { get; set; }
        protected new TModelDto WorksPageModelViewEdit { get; set; } = new TModelDto();
        protected IEnumerable<TModelDto> WorksPageModelDtoList { get; set; }
        protected override async Task OnInitializedAsync()
        {
            //  await base.OnInitializedAsync();
            if (IsInitialized) return;
           
            await this.InitializeComponentsAsync();
            IsInitialized = true;
        }
        protected async Task InitializeComponentsAsync()
        {
            await LoadingTaskService.StartTaskAsync(async (task) =>
            {
                await this.OnFindAllAsync();
                WorksPageModelDtoList = OnGetAllList();

            }, "on-panel-context", "Inicializando e Mapeando Tabelas...", "Aguarde..");
         
        }

        protected override async Task OnPageRefreshAsync()
        {
            await base.OnPageRefreshAsync();
            await RefreshGridAsync();
            OnGetAllList();
            OnHideModal(WorksPageModelModal);
        }
        protected override async Task RefreshGridAsync()
        {
            if (WorksPageModelGrid != null) await WorksPageModelGrid.ReloadAsync(WorksPageModelDtoList);

        }
        protected new IEnumerable<TModelDto> OnGetAllList()
        {

            var getAllList = base.OnGetAllList();
            if (getAllList == null) return null;
            var asQueryable = getAllList.AsQueryable().MapTo<TModelDto>();
            var result = asQueryable;
            WorksPageModelDtoList = result;
            return result;
        }
        public virtual TModelDto GetEntityDto(int? idModel)
        {
            return WorksPageModelDtoList.SingleOrDefault(f => f.Id == idModel) ?? new TModelDto();
        }

        protected void ShowDialog(TModelDto model, WorksPageModelAction actionModel)
        {
            this.IsVisible = true;
            WorksPageModelViewEdit = OnGetAllList().FirstOrDefault(f => f.Id == model?.Id) ?? new TModelDto();
            WorksPageModelAction = actionModel;
            this.OnShowModal(WorksPageModelModal);
            this.OnChanged();
        }

        protected Task<bool> OnSaveOrUpdateAsync(TModelDto saveEntityDto, bool question = true)
        {
            var entityToSave = ObjectMapper.Map<TModel>(saveEntityDto);
            return base.OnSaveOrUpdateAsync(entityToSave);
        }
        protected Task<bool> OnDeleteAsync(TModelDto deleteEntityDto, bool question = true)
        {
            var deleteEntity = ObjectMapper.Map<TModel>(deleteEntityDto);
            return base.OnDeleteAsync(deleteEntity);
        }


    }

    public abstract class WorksPageModel<TModel> : WorksPage where TModel : class, IEntity<int>, new()
    {
        
        [Inject] protected IWorksLookUpService LookUpService { get; set; }
        [Inject] protected IObjectMapper ObjectMapper { get; set; }
        [Inject] protected WorksPageDataTransfer WorksPageDataTransfer { get; set; }
        [Inject] protected IUnitOfWorkManager UnitOfWorkManager { get; set; }
        [Inject] protected IRepository<TModel> DataContext { get; set; }
       // protected WorksLoading WorksLoadingPageContext { get; set; }
        public Expression<Func<TModel, bool>> Filter { get; set; } = null;
        protected virtual IGridRefreshDataSource WorksPageModelGrid { get; set; }
        protected string Caption { get; set; }

        protected WorksModal WorksPageModelModal { get; set; }
        protected WorksPageModelAction WorksPageModelAction { get; set; } = WorksPageModelAction.New;

        protected TModel WorksPageModelViewEdit { get; set; } = new TModel();
        protected IEnumerable<TModel> WorksPageModelEntities { get; set; }

        protected string GetTitle => typeof(TModel).Name;
        protected string GetCaption => $"Manutenção do cadastro de {typeof(TModel).Name}";

        protected override Task OnAfterRenderAsync(bool firstRender)
        {

            if (!IsInitialized) IsInitialized = firstRender;
            return base.OnAfterRenderAsync(firstRender);

        }
        protected override async Task OnInitializedAsync()
        {

            if (IsInitialized) return;
            await base.OnInitializedAsync();
            await this.OnFindAllAsync();
            IsInitialized = true;
        }


        protected void ShowDialog(TModel model, WorksPageModelAction actionModel)
        {
            this.IsVisible = true;
            this.Init(model, actionModel);
            this.OnShowModal(WorksPageModelModal);
            this.OnChanged();
        }

        public virtual TModel GetEntity(int? idModel)
        {
            return WorksPageModelEntities.SingleOrDefault(f => f.Id == idModel) ?? new TModel();
        }
        public virtual void Init(TModel model, WorksPageModelAction actionModel)
        {
            if (model != null) WorksPageModelViewEdit = GetEntity(model.Id);
            WorksPageModelAction = actionModel;
        }
        protected void OnHideModal(WorksModal workModal)
        {
            if (workModal == null) return;
            if (workModal.IsVisible) workModal.Hide();
        }
        protected void OnShowModal(WorksModal workModal)
        {
            workModal?.Show();
        }

        protected IEnumerable<TModel> OnGetAllList()
        {
            return WorksPageModelEntities;
        }

        protected virtual async Task RefreshGridAsync()
        {
            if (WorksPageModelGrid != null) await WorksPageModelGrid.ReloadAsync(WorksPageModelEntities);

        }
        protected virtual async Task OnPageRefreshAsync()
        {
            await this.OnFindAllAsync();
            await RefreshGridAsync();
            OnHideModal(WorksPageModelModal);
        }


        //[UnitOfWork]
        public async Task OnFindAllAsync()
        {
            await LoadingTaskService.StartTaskAsync(async (task) =>
            {
                var uow = UnitOfWorkManager.Begin();
                    WorksPageModelEntities = new List<TModel>();
                    var result = Filter == null ? await DataContext.GetAllListAsync() : await DataContext.GetAllListAsync(Filter);
                    WorksPageModelEntities = result.ToList();
                await uow.CompleteAsync();
            }, "on-panel-context", "Consultando registros...", "Aguarde..");
        }


        protected virtual async Task<bool> OnSaveOrUpdateAsync(TModel saveEntity, bool question = true)
        {

            if (!await Validate(saveEntity)) return false;
            if (question) if (!await QuestionAsync($"Salvar as alterações para o registro:  {saveEntity} ?")) return false;

            try
            {
                await DataContext.FlushChangesAsync();
                await DataContext.InsertOrMergeAsync(saveEntity);


            }
            catch (Exception ex)
            {
                await ErrorAsync($"Erro {ex.Message}");
                return false;
            }

            ShowAlert("Registro Gravado", AlertMode.Success);
            await OnPageRefreshAsync();
            return true;
        }

        protected virtual async Task<bool> OnDeleteAsync(TModel deleteEntity)
        {


            if (!await QuestionAsync($"Deletar o registro {deleteEntity.Id}-{deleteEntity}?")) return false;
            if (deleteEntity.IsTransient())
            {
                Toast.ShowInfo("Registro não localizado!", "Deletando...");
                return false;
            }
            try
            {
                await DataContext.DeleteAsync(deleteEntity);
            }
            catch (Exception ex)
            {
                var erro = $"O seguinte erro ocorreu ao DELETAR o registro solicitado:<br> <b>{ex.Message}</b>";
                await ErrorAsync(erro);
                return false;
            }
            Toast.ShowInfo("registro deletado...", "Deletando...");
            ShowAlert("Registro Deletado", AlertMode.Warning);
            await OnPageRefreshAsync();
            return true;
        }


        protected async Task<bool> Validate(object objectValidation)
        {
            var errorList = new StringBuilder();
            if (objectValidation is IValidation modelValidation)
            {
                var errorMessages = modelValidation.Validate();
                if (errorMessages.Count <= 0) return true;
                foreach (var result in errorMessages)
                {
                    var item = $"Campo: <b>{result.MemberNames.First().ToString()}</b> Erro: <b>{result.ErrorMessage}</b></br>";
                    errorList.AppendLine(item);
                }
            }

            await ErrorAsync(errorList.ToString());
            return false;
        }
    }

}
