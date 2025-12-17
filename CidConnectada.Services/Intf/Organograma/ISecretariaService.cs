using System.Collections.Generic;
using System.Threading.Tasks;
using CidConnectada.Entities.Model.Banners;
using CidConnectada.Entities.Model.Organograma;
using CidConnectada.Webapi.Models.Organograma;
using Zenite.Pi.Services;

namespace CidConnectada.Services.Intf.Organograma
{
    public interface ISecretariaService : ICadastroMasterService<Secretaria, string, SecretariaMenu, SecretariaMenuKey>
    {
        Task<IList<Secretaria>> GetHome(int? qtde = null);

        [TransactionRequired]
        Task AlterarOrdemHome(IList<OrdemHomeDto<string>> ordemList);

        Task<IList<Secretaria>> GetActive();
        Task<IList<RotaInterna>> GetRotasInternasAsync();
    }
}