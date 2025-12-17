using System.Collections.Generic;
using System.Threading.Tasks;
using CidConnectada.Entities.Model.Emprego;
using CidConnectada.Entities.Model.Enums;
using Zenite.Pi.Services;

namespace CidConnectada.Services.Intf.Emprego
{
    public interface IOfertaVagaService : ICadastroService<OfertaVaga, long>
    {
        Task<IList<string>> SugerirDetalheEmprego(string termo, int limite, TipoEmpregoDetailEnum tipoEmpregoDetail);

        [TransactionRequired]
        TDetail GetTDetailByName<TDetail>(string nome) where TDetail : EmpregoDetail;

        FaixaSalarial GetFaixaSalarial(int id);
        Task<IList<FaixaSalarial>> GetAllFaixaSalarial();
        Task<IList<OfertaVaga>> GetMinhasVagas();
    }
}