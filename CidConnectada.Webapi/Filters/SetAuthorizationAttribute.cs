using System.Net;
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
    // NÃO FUNCIONOU...
    public class SetUserAuthorizationAttribute : AuthorizationFilterAttribute
    {
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
            if (isAsync)
                await base.OnAuthorizationAsync(actionContext, cancellationToken);
            else
                base.OnAuthorization(actionContext);

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