using System.Web.Mvc;

namespace CidConnectada.Webapi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            // System.Web.Mvc.IAuthorizationFilter versus System.Web.Http.Filters.IAuthorizationFilter
            //filters.Add(new SetAuthorizationAttribute());
        }
    }
}
