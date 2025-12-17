using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Services.Intf.Account;
using Zenite.Pi.Context;
using Zenite.Pi.IoC;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Website.Filters
{
    public class RequireHttpsAttribute : AuthorizationFilterAttribute
    {
        public RequireHttpsAttribute()
        {
            Port = 443;
        }

        public int Port { get; set; }

        #region Services

        protected IUsuarioService accountService => ApplicationContext.Resolve<IUsuarioService>();

        #endregion

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            OnAuthorizationDynamic(actionContext, new CancellationToken(), false).Wait();
        }

        public override async Task OnAuthorizationAsync(HttpActionContext actionContext,
            CancellationToken cancellationToken)
        {
            await OnAuthorizationDynamic(actionContext, cancellationToken, true);
        }

        public async Task OnAuthorizationDynamic(HttpActionContext actionContext, CancellationToken cancellationToken,
            bool isAsync)
        {
            var request = actionContext.Request;

            if (request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                var response = new HttpResponseMessage();

                if (request.Method == HttpMethod.Get || request.Method == HttpMethod.Head)
                {
                    var uri = new UriBuilder(request.RequestUri);
                    uri.Scheme = Uri.UriSchemeHttps;
                    uri.Port = Port;

                    response.StatusCode = HttpStatusCode.Found;
                    response.Headers.Location = uri.Uri;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.Forbidden;
                }

                actionContext.Response = response;
            }
            else
            {
                if (isAsync)
                    await base.OnAuthorizationAsync(actionContext, cancellationToken);
                else
                    base.OnAuthorization(actionContext);
            }

            if (actionContext.RequestContext.Principal.Identity.IsAuthenticated)
            {
                var principal = actionContext.RequestContext.Principal;
                var user = accountService.FindByUsername(principal.Identity.Name);
                var Context =
                    (ContextRequestMultiTenancy<int, string, int>)ApplicationContext.Resolve<ContextRequest<int, string>>();
                Context.User = (Usuario)EntityUtil.CloneSerialize(user, new string[1] { "AspNetUsers" });
                Context.IpAddress = IPAddress
                    .Parse(((HttpContextBase)actionContext.Request.Properties["MS_HttpContext"]).Request
                        .UserHostAddress).ToString();
            }
        }
    }
}