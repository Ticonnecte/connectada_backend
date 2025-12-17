using CidConnectada.Entities.Model.Banners;
using CidConnectada.Entities.Model.Comercios;
using CidConnectada.Services.Intf.AWS;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenite.Pi.Services;

namespace CidConnectada.Services.Intf.Comercios
{
    public interface IComercioService : ICadastroMasterService<
        Comercio,
        string,
        ComercioCategoriaVinculo,
        ComercioCategoriaVinculoKey
    >, IS3FileServiceGeneric<Comercio>
    {
        Task<IList<Comercio>> GetByTipo(int tipo);
    }
}
