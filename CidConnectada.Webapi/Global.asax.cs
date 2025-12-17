using CidConnectada.Website.App_Start;
using log4net;
using Microsoft.SqlServer.Types;
using System;
using System.Data.Entity.SqlServer;
using System.Globalization;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CidConnectada.Webapi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private readonly static ILog log = LogManager.GetLogger(typeof(WebApiApplication));

        protected void Application_Start()
        {
            // original from VS template
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // block from CidConnectada...
            PropertyInfo p = typeof(HttpRuntime).GetProperty("FileChangesMonitor", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            object o = p.GetValue(null, null);
            FieldInfo f = o.GetType().GetField("_dirMonSubdirs", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
            object monitor = f.GetValue(o);
            //DisableApplicationInsightsOnDebug();

            //Microsoft.SqlServer.Types...
            //SqlServerTypes.Utilities.LoadNativeAssemblies(Server.MapPath("~/bin"));
            SqlProviderServices.SqlServerTypesAssemblyName = typeof(SqlGeography).Assembly.FullName;

            StartAppConfig.Initialize();
            log.Info("WebSite inicializado com sucesso.");
        }

        //[Conditional("DEBUG")]
        //private static void DisableApplicationInsightsOnDebug()
        //{
        //    TelemetryConfiguration.Active.DisableTelemetry = true;
        //}

        // Allow CORS...
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("pt-BR");
            System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.Flush();
            }
        }
    }
}
