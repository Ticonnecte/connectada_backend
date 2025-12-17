using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Entities.Model.Organograma;
using CidConnectada.Services.Impl.Identity;
using CidConnectada.Services.Intf.Account;
using CidConnectada.Services.Intf.Organograma;
using CidConnectada.Webapi.Controllers;
using CidConnectada.Webapi.Models;
using CidConnectada.Webapi.Models.Account;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Zenite.Pi.Context;
using Zenite.Pi.Exceptions;
using Zenite.Pi.IoC;

namespace CidConnectada.Webapi.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        private IPrefeituraService PrefeituraService => ApplicationContext.Resolve<IPrefeituraService>();
        private ContextRequestMultiTenancy<int, string, int> Context => (ContextRequestMultiTenancy<int, string, int>)ApplicationContext.Resolve<ContextRequest<int, string>>();

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null) throw new ArgumentNullException("publicClientId");

            _publicClientId = publicClientId;
        }

        // Método de autenticação!
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                var data = await context.Request.ReadFormAsync();
                var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
                var appUser = await userManager.FindAsync(context.UserName, context.Password);
                
                if (appUser == null)
                {
                    context.SetError($"{(int)AuthErrorEnum.User_Or_Password_Incorrect}",
                        $"{AuthErrorEnum.User_Or_Password_Incorrect}");

                    return;
                }

                //Permite login do SA
                var rolesList = await userManager.GetRolesAsync(appUser.Id);
                if (rolesList.Contains("SA"))
                    Context.CacheRequest["TenantId"] = 0;
                
                var user = AccountService.FindByUsername(appUser.UserName);
                Device device = null;
                // if (!appUser.EmailConfirmed)
                // {
                //     context.SetError($"{(int)AuthErrorEnum.Email_Verification_Required}",
                //         $"{AuthErrorEnum.Email_Verification_Required}");
                //     return;
                // }

                if (user is Cidadao)
                {
                    if (!appUser.PhoneNumberConfirmed)
                    {
                        context.SetError($"{(int)AuthErrorEnum.Phone_Verification_Required}",
                            $"{AuthErrorEnum.Phone_Verification_Required}");

                        return;
                    }

                    var validGuid = Guid.TryParse(data["device_id"], out var deviceId);
                    if (data["device_id"] == null)
                    {
                        context.SetError($"{(int)AuthErrorEnum.Device_Id_Required}",
                            $"{AuthErrorEnum.Device_Id_Required}");

                        return;
                    }

                    if (!validGuid || deviceId == Guid.Empty)
                    {
                        context.SetError($"{(int)AuthErrorEnum.Invalid_GUID_In_Device_Id}",
                            $"{AuthErrorEnum.Invalid_GUID_In_Device_Id}");

                        return;
                    }

                    device = user.RefreshTokenSet.FirstOrDefault(rt => rt.Device.Key == deviceId)?.Device;

                    //if (device is null)
                    //{
                    //    context.SetError($"{(int)AuthErrorEnum.Device_Id_Mismatch}",
                    //        $"{AuthErrorEnum.Device_Id_Mismatch}");

                    //    return;
                    //}
                }

                var oAuthIdentity = await appUser.GenerateUserIdentityAsync(userManager,
                    OAuthDefaults.AuthenticationType);

                var cookiesIdentity = await appUser.GenerateUserIdentityAsync(userManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                var properties = OAuthHelper.CreateProperties(appUser, user, device);
                var ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
                context.Request.Context.Authentication.SignIn(cookiesIdentity);
            }
            catch (Exception exc)
            {
                var error = new PiBusinessException(exc);
                log.Error(error.Message);
                context.SetError(error.Message);
            }
        }

        public override async Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var form = await context.Request.ReadFormAsync();
            Guid.TryParse(form["refresh_token"], out var refreshTokenId);
            var validGuid = Guid.TryParse(form["device_id"], out var deviceId);

            var token = await AccountService.FindRefreshTokenAsync(refreshTokenId);

            if (token == null)
            {
                context.SetError($"{(int)AuthErrorEnum.Refresh_Token_Not_Found}",
                    $"{AuthErrorEnum.Refresh_Token_Not_Found}");

                return;
            }

            if (token.ExpiresUtc < DateTime.UtcNow)
            {
                context.SetError($"{(int)AuthErrorEnum.Refresh_Token_Expired}",
                    $"{AuthErrorEnum.Refresh_Token_Expired}");

                return;
            }

            if (form["device_id"] == null)
            {
                context.SetError($"{(int)AuthErrorEnum.Device_Id_Required}",
                    $"{AuthErrorEnum.Device_Id_Required}");

                return;
            }

            if (!validGuid || deviceId == Guid.Empty)
            {
                context.SetError($"{(int)AuthErrorEnum.Invalid_GUID_In_Device_Id}",
                    $"{AuthErrorEnum.Invalid_GUID_In_Device_Id}");

                return;
            }

            //if (deviceId != token.Device.Key)
            //{
            //    context.SetError($"{(int)AuthErrorEnum.Device_Id_Mismatch}",
            //        $"{AuthErrorEnum.Device_Id_Mismatch}");

            //    return;
            //}

            await base.GrantRefreshToken(context);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null) context.Validated();

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                var expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri) context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public async override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            var user = AccountService.FindByUsername(context.Identity.Name);
            var roles = user.AspNetUsers.AspNetUserRolesSet
                .Select(r => new
                {
                    id = r.RoleId,
                    name = r.AspNetRoles.Name
                }).ToList();

            var permissionsIni =
                JsonConvert.DeserializeObject<PermissionsDto>(user.AspNetUsers.AspNetUserRolesSet.First().AspNetRoles
                    .Permissions);

            foreach (var userRole in user.AspNetUsers.AspNetUserRolesSet.Skip(1))
            {
                var role = userRole.AspNetRoles;
                var newPermissions = JsonConvert.DeserializeObject<PermissionsDto>(role.Permissions);
                foreach (var current in newPermissions.permissions)
                    if (permissionsIni.permissions.TryGetValue(current.Key, out var permission))
                        foreach (var p in current.Value)
                            if (permission.TryGetValue(p.Key, out var value))
                            {
                                permission.Remove(p.Key);
                                permission.Add(p.Key, p.Value || value);
                            }
                            else
                            {
                                permission.Add(p.Key, p.Value);
                            }
                    else
                        permissionsIni.permissions.Add(current.Key, current.Value);
            }

            IDictionary<string, object> data = new Dictionary<string, object>();

            data.Add("id", user.Key);
            data.Add("tenantId", user.TenantKey);
            data.Add("tenantNome", user.Prefeitura?.Name);
            data.Add("email", user.AspNetUsers.Email);
            data.Add("telefone", user.AspNetUsers.PhoneNumber);
            data.Add("roles", !roles.Any() ? null : JsonConvert.SerializeObject(roles));
            data.Add("permissions", permissionsIni is null ? null : JsonConvert.SerializeObject(permissionsIni));
            data.Add("nome", user.Nome);
            data.Add("sobrenome", user.Sobrenome);
            data.Add("nomeCompleto", !String.IsNullOrWhiteSpace(user.NomeCompleto) ? user.NomeCompleto : null);
            data.Add("cpf", user.Cpf);
            data.Add("rg", user.Rg);
            data.Add("orgaoExpedidor", user.OrgaoExpedidor);

            if (roles.Any(r => r.name == "SA") && context.OwinContext.Request.Headers.TryGetValue("TenantId", out string[] values))
            {
                int tenantId = Int32.Parse(values.First());
                Prefeitura tenant = await PrefeituraService.ObterAsync(tenantId, null);

                data["tenantId"] = tenant.Key;
                data["tenantNome"] = tenant.Nome;
            }

            if (user is Cidadao cidadao)
            {
                data.Add("bairroId", cidadao.Bairro?.Key);
                data.Add("bairroNome", cidadao.Bairro?.Nome);
            }

            foreach (var item in data) context.AdditionalResponseParameters.Add(item.Key, item.Value);

            foreach (var property in context.Properties.Dictionary)
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
        }

        #region Services

        protected readonly ILog log = LogManager.GetLogger(typeof(WindsorConfiguration));
        protected IUsuarioService AccountService
        {
            get => ApplicationContext.Resolve<IUsuarioService>();
        }

        #endregion
    }
}