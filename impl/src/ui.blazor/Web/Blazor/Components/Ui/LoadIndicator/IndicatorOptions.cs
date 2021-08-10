using System;
using System.Collections.Generic;
using System.Text;

namespace Works.Web.Blazor.Components.Ui.LoadIndicator
{
    public class IndicatorOptions
    {
        public Type IndicatorParadigma { get; set; } = typeof(DefaultParadigma);

        public IndicatorChildContentHideModes ChildContentHideMode { get; set; } = IndicatorChildContentHideModes.CssDisplayNone;
    }

    public enum IndicatorChildContentHideModes
    {
        CssDisplayNone = 0,
        CssVisibilityHidden = 1,
        RemoveFromTree = 2
    }
}
