using System.Collections.Generic;
using System.Threading.Tasks;
using CidConnectada.Entities.Model.Comunicacao;
using CidConnectada.Entities.Model.Dto;
using Zenite.Pi.Services;

namespace CidConnectada.Services.Intf.Comunicacao
{
    public interface IEnqueteService : ICadastroMasterService<Enquete, int, EnqueteOpcao, EnqueteOpcaoKey>
    {
        [TransactionRequired]
        IList<EnqueteResposta> IncluirEnqueteResposta(IList<EnqueteResposta> respostas);

        IList<EnqueteResposta> GetRespostasDoUsuario(int enqueteId, int usuarioId);
        Task<bool> EstaRespondida(int enqueteId, int usuarioId);
        Task<EnqueteResultadoDto> GetResultado(int enqueteId);
    }
}