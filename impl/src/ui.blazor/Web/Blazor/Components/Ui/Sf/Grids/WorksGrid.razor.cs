using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Grids;
using Works.Domain.Entities;

namespace Works.Web.Blazor.Components.Ui.Sf.Grids
{
    public class BaseWorkGrid<TModel> : WorksGridComponent<TModel>  
    {
        protected SfGrid<TModel> SfGridView { get; set; }
        [Parameter] public RenderFragment GridCommand { get; set; }
        [Parameter] public EventCallback<CommandClickEventArgs<TModel>> OnCommand { get; set; }

        public async Task OnCommandClicked(CommandClickEventArgs<TModel> args)
        {
            await OnCommand.InvokeAsync(args);
        }

        public SfGrid<TModel> InternalControl => SfGridView;
        public override Task ReloadAsync(object data)
        {
            var dataSource = data as IEnumerable<TModel>;
            this.DataSource = dataSource;
            this.SfGridView.Refresh();
            this.ShouldRenderize();
            return Task.CompletedTask;
        }
       

    }

   
       

}
