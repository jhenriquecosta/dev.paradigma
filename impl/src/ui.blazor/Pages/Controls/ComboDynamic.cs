using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.DropDowns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Works.Pages.Controls
{

    public class ComboDynamic<TValue, TItem> : OwningComponentBase
    {
        [Parameter] public IEnumerable<TItem> Items { get; set; }
        [Parameter] public string ItemText { get; set; }
        [Parameter] public string ItemValue { get; set; }
        [Parameter] public string Caption { get; set; }
        [Parameter] public TValue Value { get; set; }
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenComponent<SfComboBox<TValue, TItem>>(1);
            builder.AddAttribute(2, "Placeholder", Caption);
            builder.AddAttribute(3, "DataSource", Items);
            builder.AddAttribute(4, "Value", Value);
            builder.AddAttribute(5, "ChildContent", (RenderFragment)((settings) =>
            {
                var seq = 0;
                settings.OpenComponent<ComboBoxFieldSettings>(seq);
                settings.AddAttribute(++seq, "Text", ItemText);
                settings.AddAttribute(++seq, "Value", ItemValue);
                settings.CloseComponent();
            }));
            builder.CloseComponent();

        }
    }
    public class DropDownDynamic<TValue, TItem> : OwningComponentBase
    {
        [Parameter] public IEnumerable<TItem> Items { get; set; }
        [Parameter] public string ItemText { get; set; }
        [Parameter] public string ItemValue { get; set; }
        [Parameter] public string Caption { get; set; }
        [Parameter] public TValue Value { get; set; }
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenComponent<SfDropDownList<TValue, TItem>>(1);
            builder.AddAttribute(2, "Placeholder", Caption);
            builder.AddAttribute(3, "DataSource", Items);
            builder.AddAttribute(4, "Value", Value);
            builder.AddAttribute(5, "ChildContent", (RenderFragment)((settings) =>
            {
                var seq = 0;
                settings.OpenComponent<DropDownListFieldSettings>(seq);
                settings.AddAttribute(++seq, "Text", ItemText);
                settings.AddAttribute(++seq, "Value", ItemValue);
                settings.CloseComponent();
            }));
            builder.CloseComponent();
        }
    }
   
}
