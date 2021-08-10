﻿using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Works.Web.Blazor.Components.Ui.AgGrid
{
    public partial class AgGridRow : ComponentBase
    {
        [CascadingParameter(Name = nameof(AgGridOptions.RowData))]
        public List<object> RowData { get; set; }
        
        [Parameter] public object Data { get; set; }

        protected override void OnInitialized()
        {
            if (RowData == null)
                throw new InvalidOperationException("missing required row data cascading parameter");
            if (Data == null)
                throw new InvalidOperationException("missing required data parameter");

            RowData.Add(Data);
        }
    }
}
