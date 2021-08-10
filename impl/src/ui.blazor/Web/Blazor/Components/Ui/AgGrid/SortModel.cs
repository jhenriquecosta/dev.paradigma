using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Works.Web.Blazor.Components.Ui.AgGrid
{
    public class SortModel
    {
        [JsonPropertyName("colId")]
        public string ColumnId { get; set; }

        [JsonPropertyName("sort")]
        public string Direction { get; set; }
    }
}
