using System;
using System.Collections.Generic;
using System.Text;

namespace Works
{
    public static class WorksBlazorApp
    {
        public static DateTime? GetCurrentDate()
        {
            return DateTime.Now;
        }
        public static int GetCurrentYear()
        {
            return DateTime.Now.Year;
        }
    }
}
