using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Works.Web.Blazor.Components.Ui.AgGrid
{
    /// <summary>
    /// Strongly-typed representation of:
    ///   https://www.ag-grid.com/javascript-grid-column-properties/
    /// </summary>
    public class AgGridColumnDefinition
    {
        public string Field { get; set; }

        public string HeaderName { get; set; }

        [JsonPropertyName("resizable")]
        public bool IsResizable { get; set; }

        [JsonPropertyName("sortable")]
        public bool IsSortable { get; set; }

        [JsonPropertyName("filter")]
        public bool IsFiltered { get; set; }
        
        [JsonPropertyName("cellClass")]
        public string CssClass { get; set; }
    }
}
