using Castle.Facilities.Logging;
using CurrieTechnologies.Razor.SweetAlert2;


using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using System;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.RegularExpressions;
 
using Works.AspNetCore;
 

using DevExpress.XtraReports.Web.Extensions;
using DevExpress.Blazor.Reporting;

using Works.PlugIns;
using System.IO;
using Microsoft.AspNetCore.Identity;
 
using Works.IO;

using Works.Log4Net;
using Works.Application.Configuration;
using Works.Application.Reports;

namespace Works
{
    public class Startup
    {

        IWebHostEnvironment _webEnviroment;
        private readonly IConfiguration _appConfiguration;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _webEnviroment = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSignalR(e => { e.MaximumReceiveMessageSize = 102400000; });
            services.AddRazorPages();
            services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; }).AddHubOptions((o) => { o.MaximumReceiveMessageSize = 1024 * 1024 * 100; });
            services.AddMvc().AddNewtonsoftJson(
                options =>
                {
                    options.SerializerSettings.Formatting = Formatting.Indented;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });

            services.AddScoped<HttpClient>();
            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
            services.AddResponseCompression(opts => { opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" }); });

            services.AddWorksWebBlazor();
            services.AddWorksBlazorReporting();

            services.AddWorksWebWithoutCreatingServiceProvider<AppBlazorModule>(options =>
            {
                var path_plugins = Path.Combine(_webEnviroment.ContentRootPath, WorksWebAppConsts.Folders.Default, WorksWebAppConsts.Folders.Plugins);
                WorksFile.CreateFolder(path_plugins);
                options.IocManager.IocContainer.AddFacility<LoggingFacility>(f => f.UseWorksLog4Net().WithConfig("log4net.config"));
                options.PlugInSources.AddFolder(path_plugins, SearchOption.AllDirectories);
            });
        }

        public void ConfigureDevelopment(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            CommonConfig(app);
        }
        public void ConfigureProduction(IApplicationBuilder app)
        {
            app.UseExceptionHandler("/Error");
            CommonConfig(app);
        }
        private void CommonConfig(IApplicationBuilder app)
        {
          
            app.UseWorksWeb(); // Initializes Works framework.
            app.UseUnitOfWork();
            app.UseDevExpressBlazorReporting();

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseResponseCompression();
            app.UseRouting();

          //  if (app.ApplicationServices.GetService<ISessionFactory>() == null) throw new HibernateConfigException("Unable to initialize the session factorey.");


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");

            });
            WorksLog4Net.Instance.Log.Info("services is on");


        }

    }
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddWorksBlazorReporting(this IServiceCollection services)
        {
            services.AddDevExpressBlazorReporting();
            services.AddScoped<ReportStorageWebExtension, WorksReportStorageWebExtension>();
            return services;

        }
        //public static IdentityBuilder AddWorksIdentityBuilder(this IServiceCollection services)
        //{
        //    return IdentityRegistrar.Register(services);
        //}
        //public static IServiceCollection AddWorksIdentityServer4(this IServiceCollection services, IConfigurationRoot configuration)
        //{
        //    IdentityServerRegistrar.Register(services,configuration);
        //    return services;

        //}
    }
    
}
