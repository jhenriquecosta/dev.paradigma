using System;
using System.Collections.Generic;
using System.Text;

namespace Works.Web.Blazor.Components.Ui.AgGrid
{
    public class AgGridRowData
    {
        private List<object> _Rows = new List<object>();

        public bool HasRows => _Rows != null;

        public int Count => _Rows?.Count ?? 0;

        public IEnumerable<object> Rows => _Rows;

        public void Add(object row) => _Rows.Add(row);

        public void AddRange(IEnumerable<object> rows) => _Rows.AddRange(rows);
    }
}
