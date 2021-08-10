using System;
using System.Threading.Tasks;
using Works.Web.Blazor.Common;

namespace Works.Web.Blazor.Components.Ui.LoadIndicator
{
    public interface ILoadingService
    {
        //Type DefaultParadigmaType { get; set; }
        Task StartTaskAsync(Func<ITaskStatus, Task> action, string context = "", string maintext = null, string subtext = null);
        void SubscribeToTaskProgressChanged(string context, Func<ITaskStatus, Task> action);
        void UnsubscribeFromTaskProgressChanged(string context, Func<ITaskStatus, Task> action);
        IndicatorOptions Options { get; }
    }
}
