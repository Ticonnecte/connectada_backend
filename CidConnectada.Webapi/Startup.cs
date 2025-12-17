using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(CidConnectada.Webapi.Startup))]

namespace CidConnectada.Webapi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //HttpConfiguration config = new HttpConfiguration();

            //// Web API routes
            //config.MapHttpAttributeRoutes();

            ConfigureAuth(app);

            //app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            //app.UseWebApi(config);
        }
    }
}
