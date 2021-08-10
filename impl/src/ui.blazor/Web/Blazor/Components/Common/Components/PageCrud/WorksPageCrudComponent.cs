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

   

    public class WorksPageCrudComponent<TEntity> : WorksPageCrudComponentBase<TEntity> where TEntity : class, IEntity<int>, IWorksClassGeneric<int>, new()
    {
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            builder.OpenRegion(this.GetHashCode());
            builder.AddContent(1, DataPageCrudComponent);
            builder.AddContent(2, EditFormComponent());
            builder.CloseRegion();

        }
    }
    public class WorksPageCrudComponentDto<TEntity, TModel> : WorksPageCrudComponentBase<TEntity, TModel> where TEntity : class, IEntity<int>, new() where TModel : class, IWorksClassGeneric<int>, new()
    {
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            builder.OpenRegion(this.GetHashCode());
            builder.AddContent(1, DataPageCrudComponent);
            builder.AddContent(2, EditFormComponent());
            builder.CloseRegion();
        }
    }
}
