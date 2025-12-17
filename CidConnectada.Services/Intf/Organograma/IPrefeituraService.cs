using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Identity;
using CidConnectada.Entities.Model.Organograma;
using System;
using System.Threading.Tasks;
using Zenite.Pi.Services;

namespace CidConnectada.Services.Intf.Organograma
{
    public interface IPrefeituraService : ICadastroService<Prefeitura, int>, ITenantService<Prefeitura, int>
    {
        [TransactionRequired]
        Task<Prefeitura> IncluirPlusAsync(Prefeitura entity, Usuario user, ApplicationUser appUser, Delegate upload);

        Task UploadLogos(Prefeitura entity);
        string GetAWSBaseUrl(Prefeitura entity);

        [TransactionRequired]
        Task UpdateRedesSociaisAsync(Prefeitura entity);

    }
}
