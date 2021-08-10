using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.Inputs;
using System;
using System.Threading.Tasks;
using Works.Application.Services.Dto;
using Works.Extensions;

namespace Works.Web.Blazor.Components.Ui.Sf.Inputs
{

    public class WorksMultiSelect<TEntity> : WorksInputMultiSelect<int?,TEntity>
    {
         
         
    }
    public class WorksInputMultiSelect<TValue, TItem> : WorksInputDropDownList<TValue, TItem>
    {
        public WorksInputMultiSelect()
        {
            TypeDropDown = TypeDropDown.IsMultiSelect;
        }
    }

}

