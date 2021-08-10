using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Works.Web.Blazor.Components.Ui.AgGrid
{
    public partial class AgGridColumn : ComponentBase
    {
        [CascadingParameter(Name = nameof(AgGridOptions.ColumnDefinitions))]
        public List<AgGridColumnDefinition> ColumnDefinitions { get; set; }
        
        [Parameter] public string Field { get; set; }
        [Parameter] public string Header { get; set; }
        [Parameter] public bool IsResizable { get; set; }
        [Parameter] public bool IsSortable { get; set; }
        [Parameter] public bool IsFiltered { get; set; }
        [Parameter] public string CssClass { get; set; }

        protected override void OnInitialized()
        {
            ColumnDefinitions.Add(new AgGridColumnDefinition
            {
                Field = Field,
                HeaderName = Header,
                IsResizable = IsResizable,
                IsSortable = IsSortable,
                IsFiltered = IsFiltered,
                CssClass =  CssClass
            });
        }
    }
}
