using System.Collections.Generic;
using System.Threading.Tasks;
using CidConnectada.Entities.Model.Comercios;
using CidConnectada.Webapi.Models.Organograma;
using Zenite.Pi.Services;

namespace CidConnectada.Services.Intf.Comercios
{
    public interface ITipoComercioService: ICadastroMasterService<TipoComercio, int, CategoriaTipoComercio, int>
    {
        Task<IList<CategoriaTipoComercio>> GetCategoriasByTipoAsync(int key);
        Task<IList<TipoComercio>> GetHome(int? qtde = null);
        Task<CategoriaTipoComercio> GetCategoriaAsync(int key);
        CategoriaTipoComercio GetCategoria(int key);

        [TransactionRequired]
        Task AlterarOrdemHome(IList<OrdemHomeDto<int>> ordemList);
    }
}
