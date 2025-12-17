using CidConnectada.Entities.Model.Account;
using CidConnectada.Services.Intf.Account;
using log4net;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Zenite.Pi.Context;
using Zenite.Pi.IoC;

namespace CidConnectada.Webapi.Providers
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            Usuario user = UsuarioService.FindByUsername(context.Ticket.Identity.Name);

            TimeSpan refreshTokenLifetime =
            TimeSpan.FromDays(
                Int32.Parse(ApplicationContext.AppSettings["RefreshToken:ExpireTimeSpan"]));

            IOwinRequest owinContext = context.Request;
            string userAgent = owinContext.Headers.Get("User-Agent");
            IFormCollection form = await owinContext.ReadFormAsync();

            string deviceIdString = context.Ticket.Properties.Dictionary.TryGetValue("device_id", out string deviceIdVal) ? deviceIdVal : null;
            Guid.TryParse(deviceIdString, out Guid deviceId);
            //Guid.TryParse(form["device_id"], out var deviceId);
            Device device = user.RefreshTokenSet.FirstOrDefault(rt => rt.Device.Key == deviceId)?.Device;
            device = device ?? await UsuarioService.FindDeviceAsync(deviceId);

            
            string grantType = form["grant_type"];
            if (string.IsNullOrEmpty(grantType))
            {
                context.Ticket.Properties.Dictionary.TryGetValue("grant_type", out grantType);
            }
            
            if (/*!(user is Cidadao) &&*/ device is null && grantType == "password")
            {
                string deviceName = context.Ticket.Properties.Dictionary.TryGetValue("device_name", out string deviceNameVal) ? deviceNameVal : null;
                string deviceType = context.Ticket.Properties.Dictionary.TryGetValue("device_type", out string deviceTypeVal) ? deviceTypeVal : null;
                device = new Device
                {
                    Key = deviceId,
                    Name = deviceName,
                    Type = deviceType
                };

                UsuarioService.AddDevice(device);
            }

            var token = new RefreshToken
            {
                Device = device,
                User = user,
                UserAgent = userAgent,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(refreshTokenLifetime),
                ProtectedTicket = context.SerializeTicket()
            };

            var oldTokens = await UsuarioService.FindRefreshTokensAsync(deviceId);
            var oldTokensFromUser = oldTokens.Where(rt => rt.Device.Key == deviceId).ToList();
            foreach (RefreshToken oldToken in oldTokensFromUser)
                await UsuarioService.RemoveRefreshTokenAsync(oldToken.Key);

            token = UsuarioService.CreateRefreshToken(token);
            context.SetToken(token.Key.ToString("D"));
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            if (!Guid.TryParse(context.Token, out var refreshTokenId))
                return;
            
            var token = await UsuarioService.FindRefreshTokenAsync(refreshTokenId);

            if (token == null || token.ExpiresUtc < DateTime.UtcNow)
                return;
            
            context.DeserializeTicket(token.ProtectedTicket);
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            CreateAsync(context).Wait();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            ReceiveAsync(context).Wait();
        }

        #region Services

        protected readonly ILog log = LogManager.GetLogger(typeof(WindsorConfiguration));
        protected IUsuarioService UsuarioService
        {
            get => ApplicationContext.Resolve<IUsuarioService>();
        }

        #endregion
    }
}