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

     
    public class WorksDropDown<TEntity> : WorksInputDropDownList<object, TEntity, ComboBoxItemDto, int?>
    {
        public WorksDropDown()
        {
            TypeDropDown = TypeDropDown.IsDropDownList;
        }
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            await InitializeDataSourceAsync();
        }
    }
    public class WorksInputDropDown<TValue, TItem> : WorksInputDropDownList<TValue, TItem>
    {
        public WorksInputDropDown()
        {
            TypeDropDown = TypeDropDown.IsDropDownList;
        }
    }

}

