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
   
    public class WorksComboBox<TEntity> : WorksInputDropDownList<object, TEntity, ComboBoxItemDto,int?>
    {
        public WorksComboBox()
        {
            TypeDropDown = TypeDropDown.IsComboBox;
        }
        protected override async Task OnParametersSetAsync()
        {    
            await base.OnParametersSetAsync();
            await InitializeDataSourceAsync();
        }
    }
    public class WorksInputComboBox<TValue, TItem> : WorksInputDropDownList<TValue, TItem>
    {
        public WorksInputComboBox()
        {
            TypeDropDown = TypeDropDown.IsComboBox;
        }
    }

}

