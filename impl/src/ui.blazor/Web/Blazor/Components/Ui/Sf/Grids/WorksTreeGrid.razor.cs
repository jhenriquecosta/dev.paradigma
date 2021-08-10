using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.TreeGrid;
using Works.Domain.Entities;

namespace Works.Web.Blazor.Components.Ui.Sf.Grids
{

    public class BaseWorkTreeGrid<TModel> : WorksGridComponent<TModel> 
    {
        protected IEnumerable<TModel> _datasource;
        protected SfTreeGrid<TModel> TreeGrid { get; set; }
        protected IEnumerable<TModel> GetDataSource { get => _datasource; set { _datasource = value; ShouldRenderize(); } }
        
        [Parameter] public double ColumnIndex { get; set; } = 1;
        [Parameter] public string ColumnId { get; set; } = "Id";
        [Parameter] public string ColumnParent { get; set; } = "Ancestral";   
        [Parameter] public EventCallback<TModel> OnAddItem { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (DataSource.Any()) GetDataSource = DataSource;
        }

       

        protected async Task OnGetSelectedRow(RowSelectEventArgs<TModel> args)
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
        public override Task ReloadAsync(object data)
        {
            if (this.TreeGrid.IsNull()) return Task.CompletedTask;

            this.DataSource = data as IEnumerable<TModel>;
            this.ShouldRenderize();
            this.TreeGrid.RefreshColumns();
            return Task.CompletedTask;
        }
        public void OnRefresh()
        {
            this.GetDataSource = null;
            ShouldRenderize();
        }
        public async Task CollapseAll()
        {   
            if (this.TreeGrid == null) return;
            await this.TreeGrid.ClearFiltering();
            await this.TreeGrid.CollapseAll();

        }
        public void Filter(string column,string args)
        {
            if (args.IsNullOrEmpty())
            {
                this.TreeGrid.ClearFiltering();
            }
            else
            {
                this.TreeGrid.FilterByColumn(column, "equal", args);
            }
        }
        
    }
       

}
