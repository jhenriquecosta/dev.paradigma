using System;
using System.Text.Json;

namespace Works.Web.Blazor.Components.Ui.AgGrid
{
    /// <summary>
    /// Strongly-typed counterpart of:
    ///    https://www.ag-grid.com/javascript-grid-callbacks/
    /// </summary>
    public partial class AgGridCallbacks
    {
        public Func<JsonElement, string> GetRowNodeId { set => Set(value); }

        public Func<JsonElement, string[]> GetDataPath { set => Set(value); }
    }
}
