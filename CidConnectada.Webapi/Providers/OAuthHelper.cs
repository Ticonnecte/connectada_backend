using System;
using System.Collections.Generic;
using System.Linq;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Identity;
using CidConnectada.Webapi.Models;
using Microsoft.Owin.Security;
using Zenite.Pi.Exceptions;

namespace CidConnectada.Webapi.Providers
{
    public static class OAuthHelper
    {
        public static AuthenticationProperties CreateProperties(ApplicationUser user, Usuario sysUser, Device device)
        {
            if (!sysUser.AspNetUsers.AspNetUserRolesSet.Any())
                throw new PiInfraException($"Usuário '{user.UserName}' não está associado a nenhum perfil");
            
            string deviceKey = device?.Key.ToString() is null ? "Unknown" : device.Key.ToString();
            
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", user.UserName },
                { "device_id", deviceKey },
                { "device_name", device?.Name ?? "Unknown" },
                { "device_type", device?.Type ?? "Unknown" },
            };

            return new AuthenticationProperties(data);
        }
    }
}