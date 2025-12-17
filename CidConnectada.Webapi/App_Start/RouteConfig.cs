using System.Web.Mvc;
using System.Web.Routing;

namespace CidConnectada.Webapi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                "GetFilteredColeta",
                "api/RotaColeta/GetFiltered",
                new { controller = "RotaColeta", action = "GetFiltered" }
            );
            routes.MapRoute(
                "GetFilteredPreColeta",
                "api/RotaPreColeta/GetFiltered",
                new { controller = "RotaPreColeta", action = "GetFiltered" }
            );
        }
    }
}
