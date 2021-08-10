using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Works.Web.Blazor.Components.Ui.Validation
{
    public partial class WorksValidation
    {
        [Parameter]
        public List<string> ErrorMessages { get; set; }
    }
}
