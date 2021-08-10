using System.ComponentModel;

namespace Works.Web.Enums
{

    
    public enum DialogType { Sucesso, Info, Warning, Erro }

    public enum InfoType
    {
        Default = 0,
        Crumb,
        Component,
        Description,
        Section
    }
    public enum OverlayType
    {
        Default = 0,
        Body,
        Content,
    }
    public enum FloatLabelMode
    {
        Never = 0,
        Always = 1,
        Auto = 2
    }   

    public enum WorksPageModelAction { New, Edit, Delete,Select }
    public enum ButtonStyle
    {
        [Description("none")]
        None,
        [Description("e-primary")]
        Primary,
        [Description("e-success")]
        Success,
        [Description("e-info")]
        Info,
        [Description("e-warning")]
        Warning,
        [Description("e-danger")]
        Danger,
        [Description("e-link")]
        Link
    }

  
}
