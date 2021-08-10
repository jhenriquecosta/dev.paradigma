using System;

namespace Works.Web.Blazor.Components.Ui.AgGrid
{
    /// <summary>
    /// Strongly-typed counterpart of:
    ///    https://www.ag-grid.com/javascript-grid-events/
    /// </summary>
    public partial class AgGridEvents
    {
        public Action<AgGridRowNode[]> SelectionChanged { set => Set(value); }
    }
}
