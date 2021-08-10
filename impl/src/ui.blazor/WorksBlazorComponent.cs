using System;
using System.Collections.Generic;
using System.Text;
using Syncfusion.Blazor.Grids;

namespace Works
{
    public static class WorksBlazorComponent
    {
        public static ColumnType GetTypeColumn(Type pType)
        {
            if (pType == typeof(bool)) return ColumnType.Boolean;
            if (pType == typeof(string)) return ColumnType.String;
            if (pType == typeof(decimal?) || pType == typeof(decimal)) return ColumnType.Number;
            if (pType == typeof(int?) || pType == typeof(int)) return ColumnType.Number;
            if (pType == typeof(DateTime?) || pType == typeof(DateTime)) return ColumnType.DateTime;
            return ColumnType.None;
        }

    }
}
