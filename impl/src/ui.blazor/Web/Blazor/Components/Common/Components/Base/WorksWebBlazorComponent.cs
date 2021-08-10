using System;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Syncfusion.Blazor;
using Works.Web.Blazor.Components.Ui.Sf.Toasts;
using System.ComponentModel;

namespace Works.Web.Blazor.Components.Common
{
    public enum StyleControl
    {
        [Description("e-outline")]
        Outline = 1,
        [Description("e-flat")]
        Flat = 2,
    }
    public abstract class WorksWebBlazorComponent : OwningComponentBase
    {

        

        
        protected ElementReference ElementRef { get; set; }
        
        protected object InternalCmp { get; set; }
        protected List<string> ErrorMessages { get; } = new List<string>();
     
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public Dictionary<string, object> HtmlAttributes { get; set; }
        [Parameter] public string Caption { get; set; } = string.Empty;
        [Parameter] public bool AllowEdit { get; set; } = true;
        [Parameter] public bool Disable { get; set; } = false;
        [Parameter] public bool Enabled { get; set; } = true;
        [Parameter] public string ObjectID { get; set; }
        [Parameter] public string Message { get; set; } = string.Empty;
        [Parameter] public string CssClass { get; set; } = "e-outline";
        [Parameter] public string CssIcon { get; set; }
        [Parameter] public StyleControl StyleControl { get; set; } = StyleControl.Flat;
        [Parameter] public string Width { get; set; } = string.Empty;
        [Parameter] public string Height { get; set; } = string.Empty;
        [Parameter] public string Locale { get; set; } = "pt-BR";

        public override Task SetParametersAsync(ParameterView parameters)
        {

            CssClass = StyleControl.GetDescription<StyleControl>();
            return base.SetParametersAsync(parameters);

        }

        public virtual void SetEnabled(bool enabled)
        {
            if (InternalCmp != null)
            {
                this.Enabled = enabled;
                InternalCmp.SetPropertyValue(InternalCmp.GetType(), "Enabled", this.Enabled);               
                ShouldRenderize();
            }
        }
        public virtual void ShouldRenderize()
        {
            InvokeAsync(StateHasChanged);
        }

        ~WorksWebBlazorComponent()
        {
            this.Dispose(false);
        }
        public void Dispose()
        {
            this.Dispose(true);
        }
        public string GetID()
        {
            var id = GetComponent<BaseComponent>().ID;
            return id;
        }
        public TComponent GetComponent<TComponent>() where TComponent : class
        {
            return InternalCmp as TComponent;
        }
        public void SetObjectID(Syncfusion.Blazor.BaseComponent internalComponent)
        {
            if (internalComponent == null) return;
            if (internalComponent.IsRendered) return;
            var objId = internalComponent.GetObjectId();
            if (!objId.IsNull()) return;
            if (!ObjectID.IsNullOrWhiteSpace())
            {
                internalComponent.ID = ObjectID;
            }
            else
            {
                ObjectID = internalComponent.ID;
            }
            internalComponent.SetObjectId(ObjectID);
        }
    }
}
