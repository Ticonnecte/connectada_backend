using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Lifestyle.Scoped;
using Castle.Windsor;
using Microsoft.Owin;
using System.Threading.Tasks;
using System.Web;
using Zenite.Pi.Context;
using Zenite.Pi.DependencyInjection;
using Zenite.Pi.IoC;


namespace CidConnectada.Webapi.Webapi
{
    public class WindsorScopeMiddleware : OwinMiddleware
    {
        private readonly IWindsorContainer _container;

        public WindsorScopeMiddleware(OwinMiddleware next, IWindsorContainer container)
            : base(next)
        {
            _container = container;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var httpContext = HttpContext.Current;
            if (httpContext != null && httpContext.Request.Path != "/")
            {
                OwinScope.BeginScope(context, _container);

                if (httpContext.Request.Path.ToLower() == "/api/token")
                {
                    
                    context.Response.OnSendingHeaders(state =>
                    {
                        OwinScope.Clear(context);
                    }, null);
                }
                else
                {
                    var httpContextBase = context.Get<HttpContextBase>(typeof(HttpContextBase).FullName);
                    if (httpContextBase != null)
                    {
                        httpContextBase.Items.Add(ApplicationContext.MS_OwinContext, context);
                    }
                }
            }

            await Next.Invoke(context);
        }
    }
}