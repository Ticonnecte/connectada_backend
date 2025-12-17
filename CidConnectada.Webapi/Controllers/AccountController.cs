using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using CidConnectada.Dao.Identity;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Entities.Model.Identity;
using CidConnectada.Services.Impl.Identity;
using CidConnectada.Services.Intf.Account;
using CidConnectada.Services.Intf.Organograma;
using CidConnectada.Webapi.Models;
using CidConnectada.Webapi.Providers;
using CidConnectada.Website.Filters;
using Common.Logging;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.OAuth.Messages;
using Zenite.Pi.Context;
using Zenite.Pi.Entities.Model.Common;
using Zenite.Pi.IoC;
using Zenite.Pi.Web.Models.Pesquisa;

namespace CidConnectada.Webapi.Controllers
{
    //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
    //[EnableCors("*", "*", "*")]
    [ClaimsAuthorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";

        private ApplicationUserManager _userManager;
        //private ApplicationRoleManager _roleManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            var externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null) return null;

            var logins = new List<UserLoginInfoViewModel>();

            foreach (var linkedAccount in user.Logins)
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });

            if (user.PasswordHash != null)
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName
                });

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [ClaimsAuthorize]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded) return GetErrorResult(result);

            return Ok("Password successfully changed");
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        [ClaimsAuthorize]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var operationKey = User.Identity.GetUserId();
            if (model.userKey.HasValue && await UserManager.IsInRoleAsync(operationKey, UsuarioService.GetMasterRole()))
            {
                var User = await UsuarioService.ObterAsync(model.userKey.Value);
                operationKey = User.AspNetUsers.Key;
            }

            if (await UserManager.HasPasswordAsync(operationKey)) await UserManager.RemovePasswordAsync(operationKey);

            var result = await UserManager.AddPasswordAsync(operationKey, model.NewPassword);

            if (!result.Succeeded) return GetErrorResult(result);

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            var ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null
                || ticket.Identity == null
                || ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow)
                return BadRequest("External login failure.");

            var externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null) return BadRequest("The external login is already associated with an account.");

            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded) return GetErrorResult(result);

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            else
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));

            if (!result.Succeeded) return GetErrorResult(result);

            return Ok();
        }

        // GET api/Account/ExternalLogin
        //[OverrideAuthentication]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        //[AllowAnonymous]

        //[Route("ExternalLogin", Name = "ExternalLogin")]
        //public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        //{
        //    if (error != null)
        //    {
        //        return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
        //    }

        //    if (!User.Identity.IsAuthenticated)
        //    {
        //        return new ChallengeResult(provider, this);
        //    }

        //    ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

        //    if (externalLogin == null)
        //    {
        //        return InternalServerError();
        //    }

        //    if (externalLogin.LoginProvider != provider)
        //    {
        //        Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
        //        return new ChallengeResult(provider, this);
        //    }

        //    ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
        //        externalLogin.ProviderKey));

        //    bool hasRegistered = user != null;

        //    if (hasRegistered)
        //    {
        //        Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

        //        ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
        //           OAuthDefaults.AuthenticationType);
        //        ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
        //            CookieAuthenticationDefaults.AuthenticationType);

        //        User lcUser = (User)UserService.ObterUser(user.Id);

        //        AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user, lcUser);
        //        Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
        //    }
        //    else
        //    {
        //        IEnumerable<Claim> claims = externalLogin.GetClaims();
        //        ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
        //        Authentication.SignIn(identity);
        //    }

        //    return Ok();
        //}

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            var descriptions = Authentication.GetExternalAuthenticationTypes();
            var logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (var description in descriptions)
            {
                var login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state
                    }),
                    State = state
                };

                logins.Add(login);
            }

            return logins;
        }

        // TRANSFER TO USERCONTROLLER...
        // POST api/Account/Register
        //[HttpPost]
        ////[ClaimsAuthorize(Roles ="SA,ADMIN")]
        ////[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        //[Route("Register")]
        //public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        //{
        //    try
        //    {
        //        model.naturezaJuridica = NaturezaJuridicaEnum.Física;
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }
        //        User entity = new User() {
        //            Key = 0 
        //        };
        //        string userName = model.userName;
        //        int pos = userName.IndexOf('@');
        //        string tenantDomain = "";
        //        if (pos >= 0)
        //        {
        //            tenantDomain = userName.Substring(pos + 1).ToLower();
        //            userName = userName.Substring(0, pos) + "@" + tenantDomain;
        //        }
        //        if (!(await TenantService.ExistsAsync(e => e.Domain == tenantDomain)))
        //        {
        //            throw new PiBusinessException(string.Format("Conta Contrato '{0}' não encontrada.", tenantDomain));
        //        }
        //        //else if ((!User.IsInRole("SA") && tenantDomain != ((User)Context.User).ContaContrato.Domain))
        //        //{
        //        //    throw new PiBusinessException(string.Format("Usuário '{0}' não tem permissão para registrar novo usuário em empresa diferente da atual.", Context.UserName));
        //        //}
        //        //IdentityRole role = GetRoleManager().Roles.SingleOrDefault(e => e.Id == model.RoleId.ToString());

        //        entity = AMapper.Map<User>(model);
        //        var user = new ApplicationUser()
        //        {
        //            UserName = userName,
        //            Email = model.email
        //        };

        //        IdentityResult result = await UserManager.CreateAsync(user, model.senha);

        //        if (!result.Succeeded)
        //        {
        //            return GetErrorResult(result);
        //        }
        //        else
        //        {
        //            // Desse jeito não funcionou...
        //            if (!string.IsNullOrEmpty(model.roleName))
        //            {
        //                await UserManager.AddToRoleAsync(user.Id, model.roleName);
        //            }
        //            //UserManager.AddToRole(user.Id, "1");
        //            //user.Roles.Add(new IdentityUserRole() { UserId = user.Id, RoleId = "1" });


        //            //Empresa tenant = await TenantService.ObterAsyncs(Context.TenantKey);
        //            //ContaContrato tenant = await TenantService.GetByDomain(tenantDomain);
        //            ContaContrato tenant = await TenantService.ObterAsync(Context.TenantKey);
        //            AspNetUsers aspNetUsers = await UserService.GetAspNetUsers(user.Id);
        //            entity.AspNetUsers = aspNetUsers;
        //            entity.ContaContrato = tenant;
        //            await UserService.IncluirAsync(entity);
        //            //role.Users.Add(new IdentityUserRole() { RoleId = model.RoleId.ToString(), UserId = user.Id });
        //            //await GetRoleManager().UpdateAsync(role);
        //        }
        //        //else
        //        //{
        //        //    throw new PiBusinessException(string.Format("Perfil desejado não encontrado."));
        //        //}

        //        return Ok(new { key = entity.Key, pessoaKey = entity.Pessoa != null ? entity.Pessoa.Key : 0 });
        //    }
        //    catch (Exception exc)
        //    {
        //        PiBusinessException exception = new PiBusinessException(exc);
        //        log.Error(exception);
        //        return InternalServerError(exception);
        //    }
        //}

        //POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null) return InternalServerError();

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await UserManager.CreateAsync(user);
            if (!result.Succeeded) return GetErrorResult(result);

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded) return GetErrorResult(result);
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Services

        protected IUsuarioService UsuarioService
        {
            get => ApplicationContext.Resolve<IUsuarioService>();
        }
        protected IPrefeituraService PrefeituraService
        {
            get => ApplicationContext.Resolve<IPrefeituraService>();
        }

        protected readonly ILog log = LogManager.GetLogger(typeof(WindsorConfiguration));

        protected ContextRequestMultiTenancy<int, string, int> Context =
            (ContextRequestMultiTenancy<int, string, int>)ApplicationContext.Resolve<ContextRequest<int, string>>();

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        //public ApplicationRoleManager RoleManager
        //{
        //    get
        //    {
        //        return _roleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
        //    }
        //    private set
        //    {
        //        _roleManager = value;
        //    }
        //}

        //public ApplicationRoleManager RoleManager => Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();

        protected IMapper AMapper
        {
            get => ApplicationContext.Resolve<IMapper>();
        }

        #endregion

        #region Roles

        protected RoleManager<IdentityRole> GetRoleManager()
        {
            var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
            return new RoleManager<IdentityRole>(roleStore);
        }

        // GET api/Account/GetRoles
        [HttpGet]
        [Route("GetRoles")]
        [ResponseType(typeof(IEnumerable<PiLookup<string>>))]
        public async Task<IHttpActionResult> GetRoles()
        {
            var user = await UserManager.FindByIdAsync(Context.UserOperationKey);
            var userRole = user.Roles.Last();
            try
            {
                var result = GetRoleManager().Roles.Where(e => e.Id.CompareTo(userRole.RoleId) >= 0)
                    .Select(e => new PiLookup<string>
                    {
                        Value = e.Id,
                        Text = e.Name
                    }).ToList();

                return Ok(result);
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        // GET api/Account/GetRoles
        [HttpGet]
        //[Authorize(Roles = "SA,ADMIN")]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("GetAllRoles")]
        [ResponseType(typeof(IEnumerable<RolesViewModel>))]
        public IHttpActionResult GetAllRoles()
        {
            //ApplicationUser user = await UserManager.FindByIdAsync(Context.UserKey);
            //IdentityUserRole userRole = user.Roles.Last();
            try
            {
                IList<IdentityRole> result;

                //if (Context.IsAdmin)
                //IList<IdentityRole> result = new List<IdentityRole>();
                //if (await GetRoleManager().RoleExistsAsync("SA"))
                //{
                //    result = GetRoleManager().Roles.ToList();
                //}
                //else if (await GetRoleManager().RoleExistsAsync("ADMIN"))
                //{
                //    result = GetRoleManager().Roles.Where(e => e.Name.ToUpper() != "SA" && e.Name.ToUpper() != "ADMIN").ToList();
                //}

                //await Task.Factory.StartNew(() => { });

                result = GetRoleManager().Roles.ToList();

                //List<RolesViewModel> result = GetRoleManager().Roles.Where(e => e.Id.CompareTo(userRole.RoleId) >= 0).Select(e => new PiLookup<string>() { Value = e.Id, Text = e.Name }).ToList();
                return Ok(AMapper.Map<IEnumerable<RolesViewModel>>(result));
            }
            catch (Exception exc)
            {
                return InternalServerError(exc);
            }
        }

        #endregion

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get => Request.GetOwinContext().Authentication;
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null) return InternalServerError();

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error);

                if (ModelState.IsValid)
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null) claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null) return null;

                var providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null
                    || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                    return null;

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer) return null;

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static readonly RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");

                var strengthInBytes = strengthInBits / bitsPerByte;

                var data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion

        #region Custom

        [HttpGet]
        [Route("GetTenants")]
        [ResponseType(typeof(IEnumerable<IList<PiLookup<int>>>))]
        public async Task<IHttpActionResult> GetTenants()
        {
            IList<PiLookup<int>> result = (await PrefeituraService.GetAllAsync())
                .Select(e => new PiLookup<int>
                {
                    Value = e.Key,
                    Text = e.Name,
                    Group = "Prefeituras"
                }).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Usuario user = UsuarioService.FindByUsername(model.userName);

            if (user is null)
                return BadRequest("User not found.");

            VerificacaoConta verification = user.VerificacaoConta;
            if (verification == null || verification.DataExpiracaoUtc < DateTime.UtcNow || verification.Codigo != model.code)
                return BadRequest("Invalid or expired verification code.");

            string operationKey = user.AspNetUsers.Key;
            if (await UserManager.HasPasswordAsync(operationKey))
                await UserManager.RemovePasswordAsync(operationKey);

            IdentityResult result = await UserManager.AddPasswordAsync(operationKey, model.newPassword);

            if (!result.Succeeded)
                return GetErrorResult(result);

            if (!user.AspNetUsers.Phonenumberconfirmed)
            {
                Context.User = user;
                user.AspNetUsers.Phonenumberconfirmed = true;
                await UsuarioService.AlterarAsync(user);
            }
            
            Device device = await UsuarioService.FindDeviceAsync(model.deviceId);
            if (device is null)
            {
                device = AMapper.Map<Device>(model);
                UsuarioService.AddDevice(device);
            }

            return Ok(await SignIn(user, device));
        }

        [HttpPost]
        [Route("SendVerificationCode")]
        public async Task<IHttpActionResult> SendVerificationCode(SendVerificationCodeDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = UsuarioService.FindByUsername(model.userName);

            if (user is null) return BadRequest("User not found.");

            if (model.servicoEnvioMsg == ServicoEnvioMsgEnum.WhatsApp && user.AspNetUsers.PhoneNumber == null)
                return BadRequest("O usuário informado não possui telefone cadastrado.");

            if (model.servicoEnvioMsg == ServicoEnvioMsgEnum.Email && user.AspNetUsers.Email == null)
                return BadRequest("O usuário informado não possui email cadastrado.");

            Context.User = user;
            await UsuarioService.SendVerificationCodeAsync(user, model.servicoEnvioMsg);
            return Ok("Verification Code Sent.");
        }

        [HttpPost]
        [Route("VerifyAccount")]
        public async Task<IHttpActionResult> VerifyAccount(VerifyAccountModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = UsuarioService.FindByUsername(model.userName);

            if (user is null)
                return BadRequest("User not found.");

            var verification = user.VerificacaoConta;

            if (verification == null || verification.DataExpiracaoUtc < DateTime.UtcNow || verification.Codigo != model.code) return BadRequest("Invalid or expired verification code.");

            Device device = await UsuarioService.FindDeviceAsync(model.deviceId);
            if (device is null)
            {
                device = AMapper.Map<Device>(model);
                UsuarioService.AddDevice(device);
            }

            if (!user.AspNetUsers.Phonenumberconfirmed)
            {
                Context.User = user;
                user.AspNetUsers.Phonenumberconfirmed = true;
                await UsuarioService.AlterarAsync(user);
            }

            return Ok(await SignIn(user, device));
        }

        private async Task<Dictionary<string, object>> SignIn(Usuario entity, Device device)
        {
            var owinContext = Request.GetOwinContext();
            var appUser = await UserManager.FindByNameAsync(entity.UserName);
            
            var authProperties = OAuthHelper.CreateProperties(appUser, entity, device);
            authProperties.IssuedUtc = DateTimeOffset.UtcNow;
            authProperties.ExpiresUtc = DateTimeOffset.UtcNow.Add(Startup.OAuthOptions.AccessTokenExpireTimeSpan);
            
            var identity = await appUser.GenerateUserIdentityAsync(UserManager,
                OAuthDefaults.AuthenticationType);

            var ticket = new AuthenticationTicket(identity, authProperties);
            
            var accessToken = Startup.OAuthOptions.AccessTokenFormat.Protect(ticket);
            
            var requestParams = new Dictionary<string, string[]>
            {
                { "grant_type", new[] { "custom_code" } },
            };
            var tokenRequest = new TokenEndpointRequest(new FormCollection(requestParams));
            var tokenEndpointContext = new OAuthTokenEndpointContext(
                owinContext,
                Startup.OAuthOptions,
                ticket,
                tokenRequest
            );
            
            await Startup.OAuthOptions.Provider.TokenEndpoint(tokenEndpointContext);
            
            var refreshTokenContext = new AuthenticationTokenCreateContext(owinContext,
                Startup.OAuthOptions.RefreshTokenFormat, ticket);
            
            await Startup.OAuthOptions.RefreshTokenProvider.CreateAsync(refreshTokenContext);
            var refreshToken = refreshTokenContext.Token;
            
            var response = new Dictionary<string, object>(tokenEndpointContext.AdditionalResponseParameters)
            {
                { "access_token", accessToken },
                { "token_type", "bearer" },
                { ".expires_in", (int)Startup.OAuthOptions.AccessTokenExpireTimeSpan.TotalSeconds },
                { "refresh_token", refreshToken }
            };

            return response;
        }

        [HttpGet]
        [Route("GetRolesList")]
        [ResponseType(typeof(IEnumerable<IList<PiLookup<string>>>))]
        public async Task<IHttpActionResult> GetRolesList()
        {
            IList<PiLookup<string>> result = (await UsuarioService.GetRolesListAsync())
                .Select(e => new PiLookup<string>
                {
                    Value = e.Key,
                    Text = e.Name,
                    Group = e.Description
                }).ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("RevokeDevice")]
        public async Task<IHttpActionResult> RevokeDevice(string deviceId)
        {
            if (!Guid.TryParse(deviceId, out var deviceIdGuid))
                return BadRequest($"{AuthErrorEnum.Invalid_GUID_In_Device_Id}");

            var tokens = await UsuarioService.FindRefreshTokensAsync(deviceIdGuid);
            if (tokens.Any())
            {
                foreach (var token in tokens) await UsuarioService.RemoveRefreshTokenAsync(token.Key);
                return Ok();
            }

            return BadRequest("Device not found.");
        }

        [HttpGet]
        [Route("GetSession")]
        [ResponseType(typeof(SessionDto))]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO,CIDADAO")]
        public IHttpActionResult GetSession()
        {
            var user = (Usuario)Context.User;
            if (user != null)
            {
                var sessionDto = new SessionDto
                {
                    userName = user.AspNetUsers.Username
                };

                return Ok(sessionDto);
            }

            return BadRequest("User not found.");
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[Route("RecoverPassword")]
        //public async Task<IHttpActionResult> RecoverPassword(RecoverPasswordDto model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    User user = await UserService.FindByEmailAsync(model.email);

        //    if (user is null)
        //    {
        //        return BadRequest("User not found.");
        //    }
        //    AccountVerification verification = user.AccountVerification;

        //    if (verification == null || verification.CodeExpiryDate < DateTime.UtcNow || verification.Code != model.code)
        //    {
        //        return BadRequest("Invalid or expired verification code.");
        //    }

        //    IdentityResult result = await UserManager.ResetPasswordAsync(User.Identity.GetUserId(), model.code,
        //        model.NewPassword);

        //    if (!result.Succeeded)
        //    {
        //        return GetErrorResult(result);
        //    }
        //    return Ok();
        //}

        protected IList<piLookupModel<TEnumKey>> GetEnum<TEnumKey, TEnum>(string group, bool toUpper = false)
            where TEnum : struct
        {
            IList<piLookupModel<TEnumKey>> list = new List<piLookupModel<TEnumKey>>();
            var values = Enum.GetValues(typeof(TEnum));
            Array names = Enum.GetNames(typeof(TEnum));
            for (var i = 0; i < values.Length; i++)
            {
                list.Add(new piLookupModel<TEnumKey>
                {
                    value = (TEnumKey)values.GetValue(i),
                    text = names.GetValue(i).ToString().Replace('_', ' '),
                    group = group
                });

                if (toUpper) list[i].text = list[i].text.ToUpper();
            }

            return list;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetAuthErrorsList")]
        [ResponseType(typeof(IList<piLookupModel<int>>))]
        public async Task<IHttpActionResult> GetAuthErrorsList()
        {
            return Ok(await Task.Run(() => GetEnum<int, AuthErrorEnum>("Authorization Errors", true)));
        }

        #endregion
    }
}