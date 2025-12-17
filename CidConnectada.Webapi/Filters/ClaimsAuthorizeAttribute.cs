using Castle.Windsor;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Services.Intf.Account;
using CidConnectada.Services.Intf.Organograma;
using log4net;
using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Zenite.Pi.Context;
using Zenite.Pi.Exceptions;
using Zenite.Pi.IoC;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Website.Filters
{
    public class ClaimsAuthorizeAttribute : AuthorizeAttribute
    {
        //public override void OnAuthorization(HttpActionContext actionContext)
        //{
        //    bool isAuthorize = true;
        //    var user = HttpContext.Current.User as ClaimsPrincipal;
        //    if (!string.IsNullOrEmpty(this.Roles))
        //    {
        //        isAuthorize = false;
        //        string[] roles = this.Roles.Split(',');
        //        for(int i = 0; i < roles.Length; i++)
        //        {
        //            isAuthorize = isAuthorize || user.IsInRole(roles[i]);
        //            if (isAuthorize)
        //            {
        //                break;
        //            }
        //        }
        //    }
        //    if (isAuthorize)
        //    {
        //        base.OnAuthorization(actionContext);
        //    }
        //    else
        //    {
        //        HandleUnauthorizedRequest(actionContext);
        //    }
        //}

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var authorized = base.IsAuthorized(actionContext);

            ContextRequestMultiTenancy<int, string, int> context = (ContextRequestMultiTenancy<int, string, int>) ApplicationContext.Resolve<ContextRequest<int, string>>();
            
            int headerTenantId = 0;
            if (actionContext.Request.Headers.TryGetValues("TenantId", out IEnumerable<string> tenantIdValues))
            {
                headerTenantId = Convert.ToInt32(tenantIdValues.First());
                context.CacheRequest["TenantId"] = headerTenantId;
            }

            if (authorized)
            {
                var claimsPrincipal = HttpContext.Current.User as ClaimsPrincipal;
                
                var tenantClaim = claimsPrincipal?.Claims?.FirstOrDefault(c => c.Type == "TenantId");
                
                if (tenantClaim != null && int.TryParse(tenantClaim.Value, out var claimsTenantId))
                {
                    if (!claimsPrincipal.IsInRole("SA") && headerTenantId != 0 && headerTenantId != claimsTenantId)
                        throw new PiBusinessException("TenantId informado no Header não coincide com o do usuário logado");
                
                    context.CacheRequest["TenantId"] = claimsTenantId;
                }
                
                if (context.User == null)
                {
                    var user = accountService.FindByUsername(claimsPrincipal.Identity.Name);
                    
                    context.User = (Usuario)EntityUtil.CloneSerialize(user,
                        new string[2]
                        {
                            "AspNetUsers.AspNetUserRolesSet.AspNetRoles", "Prefeitura"
                        });
                    string requestUserHostAddress = ((HttpContextBase)actionContext.Request.Properties["MS_HttpContext"]).Request
                        .UserHostAddress;
                    if (requestUserHostAddress != null)
                        context.IpAddress = IPAddress
                            .Parse(requestUserHostAddress).ToString();
                    //log.Info(string.Format("Token de autenticação validado com sucesso (IP: {0}): User.Key: {1}, Tenant.Name: {2}", Context.IpAddress, Context.UserKey, Context.TenantName));
                }
                
                if (claimsPrincipal != null && claimsPrincipal.IsInRole("SA"))
                {
                    context.CacheRequest["TenantId"] = headerTenantId;
                    ((Usuario)context.User).Prefeitura = PrefeituraService.Obter(headerTenantId);
                }
            }
            else if (headerTenantId != 0 && !Roles.Any())
            {
                authorized = true;
            }
            else
            {
                HandleUnauthorizedRequest(actionContext);
            }

            return authorized;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            var msg = "Não há nenhum Usuário logado no momento.";
            if (actionContext.Request.Headers?.Authorization != null)
                //msg = string.Format("Você não tem permissão para acessar o serviço '{0}'. | {1}", actionContext.Request.RequestUri.AbsolutePath, actionContext.Request.Headers?.Authorization?.Parameter);
                msg = String.Format("Você não tem permissão para acessar o serviço '{0}'.",
                    actionContext.Request.RequestUri.AbsolutePath);
            actionContext.Response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = new StringContent(msg)
            };
            //log.Error(msg);
        }

        #region Services

        protected IUsuarioService accountService => ApplicationContext.Resolve<IUsuarioService>();

        protected IPrefeituraService PrefeituraService => ApplicationContext.Resolve<IPrefeituraService>();

        protected readonly ILog log = LogManager.GetLogger(typeof(WindsorConfiguration));

        #endregion
    }
}