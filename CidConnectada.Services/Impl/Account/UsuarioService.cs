using Amazon.IdentityManagement.Model;
using Amazon.Util.Internal.PlatformServices;
using CidConnectada.Dao.Account;
using CidConnectada.Dao.Organograma;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Identity;
using CidConnectada.Entities.Model.Organograma;
using CidConnectada.Services.Impl.Identity;
using CidConnectada.Services.Intf.Account;
using CidConnectada.Services.Intf.Messaging;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Zenite.Pi.Context;
using Zenite.Pi.Dao;
using Zenite.Pi.Exceptions;
using Zenite.Pi.IoC;

namespace CidConnectada.Services.Impl.Account
{
    public class UsuarioService : UsuarioGenericService<Usuario, UsuarioDao>, IUsuarioService
    {
        public UsuarioService(UsuarioDao _cadDao,
            Func<ContextRequest<int, string>> contextFactory,
            AspNetUsersDao aspNetUsersDao,
            AspNetRolesDao aspNetRolesDao,
            RefreshTokenDao refreshTokenDao,
            DeviceDao deviceDao,
            PrefeituraDao prefeituraDao,
            VerificacaoContaDao verificacaoContaDao,
            IZApiService zApiService,
            Func<ApplicationUserManager> userManagerFactory
        ) : base(_cadDao,
            contextFactory,
            aspNetUsersDao,
            aspNetRolesDao,
            refreshTokenDao,
            deviceDao,
            prefeituraDao,
            verificacaoContaDao,
            zApiService,
            userManagerFactory)
        { }

        #region Custom

        public IList<Usuario> GetWhatsAppEnabled()
        {
            return cadDao.Where(u => u.AceitaMsgWhastApp).ToList();
        }
        
        public async Task<IList<Usuario>> GetWhatsAppEnabledAsync()
        {
            return await cadDao.Where(u => u.AceitaMsgWhastApp).OrderBy(u => u.NomeCompleto).ToListAsync();
        }

        #endregion
    }
}