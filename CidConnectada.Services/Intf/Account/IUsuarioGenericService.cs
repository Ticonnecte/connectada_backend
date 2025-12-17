using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Entities.Model.Identity;
using Zenite.Pi.Services;
using Zenite.Pi.Services.Intf.Account;

namespace CidConnectada.Services.Intf.Account
{
    public interface IUsuarioGenericService<TEntity> : ICadastroService<TEntity, int>, IAccountMultiTenancyService<int, string>
        where TEntity : Usuario
    {
        
        #region CRUD

        [TransactionRequired]
        Task<TEntity> IncluirAsync(TEntity entity, ApplicationUser appUser, string password);

        Task IncluirAdminAsync(TEntity entity, ApplicationUser appUser);

        #endregion

        #region Custom

        [TransactionRequired]
        Task DeleteIfPhoneNotConfirmedAsync(string username);
        Task<IList<AspNetRoles>> GetRolesListAsync();
        string GetRoleIdByName(string roleName);
        Task<AspNetUsers> GetAspNetUsers(string key);
        TEntity FindByUsername(string userName);
        Task<IList<TEntity>> GetByRole(string id);

        TEntity GetPrincipal();

        #endregion

        #region Mensaging

        Task EnviarCredenciais(Usuario entity, string password);
        
        #endregion
        
        #region AccountVerification
        
        [TransactionRequired]
        Task SendVerificationCodeAsync(Usuario user, ServicoEnvioMsgEnum srvMsg);
        
        #endregion
        
        #region RefreshToken

        [TransactionRequired]
        RefreshToken CreateRefreshToken(RefreshToken token);
        Task<IList<RefreshToken>> FindRefreshTokensAsync(Guid id);
        Task<RefreshToken> FindRefreshTokenAsync(Guid id);
        Task<RefreshToken> FindRefreshTokenAsync(Guid deviceGuidId, long userId);

        [TransactionRequired]
        Task RemoveRefreshTokenAsync(Guid id);

        [TransactionRequired]
        Task RemoveRefreshTokenAsync(IList<RefreshToken> tokens);

        #endregion
        
        #region Device
        
        Device FindDevice(Guid id);
        Task<Device> FindDeviceAsync(Guid id);
        
        [TransactionRequired]
        void AddDevice(Device device);
        
        [TransactionRequired]
        Task RemoveDeviceAsync(Device device);
        
        #endregion
        
        #region Password 
        
        string GerarSenhaForte(int length = 12);
        
        #endregion

    }
}