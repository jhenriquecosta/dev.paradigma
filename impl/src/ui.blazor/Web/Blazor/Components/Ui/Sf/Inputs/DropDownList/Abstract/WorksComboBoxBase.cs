using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Works.Application.Services;
using Works.Application.Services.Dto;

namespace Works.Web.Blazor.Components.Ui.Sf.Inputs
{
    
    public abstract class WorksComboBoxBase<TEntity,TValue> : WorksSfComponentBase<TValue>
    {
        
        [Parameter] public bool AllowFiltering { get; set; } = true;
        [Parameter] public bool IgnoreAccent { get; set; } = true;
        [Parameter] public string FieldText { get; set; } = "FieldText";
        [Parameter] public string FieldValue { get; set; } = "FieldValue";
        [Parameter] public EventCallback<TEntity> ValueChanged { get; set; }        
        [Parameter] public IEnumerable<ComboBoxItemDto> DataSource { get; set; }
        

    }
  
}
