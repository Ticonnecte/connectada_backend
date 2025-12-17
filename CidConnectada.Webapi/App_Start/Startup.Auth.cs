using System;
using CidConnectada.Dao.Identity;
using CidConnectada.Services.Impl.Identity;
using CidConnectada.Webapi.Models;
using CidConnectada.Webapi.Providers;
using CidConnectada.Webapi.Webapi;
using CidConnectada.Website.Filters;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Zenite.Pi.IoC;

namespace CidConnectada.Webapi
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            app.Use<WindsorScopeMiddleware>(ApplicationContext.container);
            //app.UseCors(CorsOptions.AllowAll);

            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            
            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            
            // Configure the application for OAuth based flow
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/api/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                RefreshTokenProvider = new RefreshTokenProvider(),
                //AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(Int32.Parse(ApplicationContext.AppSettings["AccessToken:ExpireTimeSpan"])),
                AuthorizationCodeExpireTimeSpan = TimeSpan.FromDays(Int32.Parse(ApplicationContext.AppSettings["AccessToken:AuthorizationCodeExpireTimeSpan"])),
                // In production mode set AllowInsecureHttp = false
                AllowInsecureHttp = true
            };

            
            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);
            app.UseOAuthAuthorizationServer(OAuthOptions);
            
            //app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

            //OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            //{
            //    //For Dev enviroment only (on production should be AllowInsecureHttp = false)
            //    AllowInsecureHttp = true,
            //    TokenEndpointPath = new PathString("/oauth2/token"),
            //    AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
            //    Provider = new CustomOAuthProvider(),
            //    AccessTokenFormat = new CustomJwtFormat("http://jwtauthzsrv.azurewebsites.net")
            //};

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
    }
}