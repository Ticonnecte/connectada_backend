using System.Web.Http;
using CidConnectada.Website.Filters;
using Microsoft.Owin.Security.OAuth;
using Zenite.Pi.DependencyInjection;

namespace CidConnectada.Webapi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //var conrs = new EnableCorsAttribute("*", "*", "*")
            //{
            //    SupportsCredentials = true,
            //};
            //config.EnableCors(conrs);

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);


            //GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings
            //    .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            //GlobalConfiguration.Configuration.Formatters
            //    .Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);

            //EnableCorsAttribute cors;
            //string[] ips = ConfigurationManager.AppSettings["IpsCors"].Split(';');

            //foreach(string ip in ips)
            //{
            //    cors = new EnableCorsAttribute(string.Format("http://{0}", ip), "*", "*");
            //    config.EnableCors(cors);
            //    cors = new EnableCorsAttribute(string.Format("https://{0}", ip), "*", "*");
            //    config.EnableCors(cors);
            //}

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Adiciona o ScopeCleanupHandler ao pipeline
            config.MessageHandlers.Add(new ScopeCleanupHandler());


            // Enforce HTTPS
            //config.Filters.Add(new Website.Filters.RequireHttpsAttribute());
        }
    }
}