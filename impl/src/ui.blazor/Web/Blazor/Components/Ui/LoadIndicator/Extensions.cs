using Microsoft.Extensions.DependencyInjection;
using System;

namespace Works.Web.Blazor.Components.Ui.LoadIndicator
{
    public static class Extensions
    {
        //public static IServiceCollection AddLoadingIndicator(this IServiceCollection services, bool dumpExceptionsToConsole = false)
        //{
        //    services.AddSingleton<ILoadingService, LoadingService>(_ => new LoadingService
        //    {
        //        DumpExceptionsToConsole = dumpExceptionsToConsole
        //    });
        //    return services;
        //}
        public static IServiceCollection AddLoadingIndicator(this IServiceCollection services, Action<IndicatorOptions> options = null)
        {
            var _options = new IndicatorOptions();
            options?.Invoke(_options);
            services.AddSingleton<ILoadingService, LoadingService>(_ => new LoadingService
            {
                Options = _options
            });
            return services;
        }
    }
}