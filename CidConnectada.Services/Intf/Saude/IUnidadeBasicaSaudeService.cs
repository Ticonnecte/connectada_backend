using CidConnectada.Dao.Saude;
using CidConnectada.Entities.Model.Saude;
using CidConnectada.Services.Intf.AWS;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenite.Pi.Services;
using Zenite.Pi.Services.Impl;

namespace CidConnectada.Services.Intf.Saude
{
    public interface IUnidadeBasicaSaudeService : IS3FileServiceGeneric<UnidadeBasicaSaude>, 
        ICadastroMasterService<
            UnidadeBasicaSaude,
            string,
            UbsEspecialidadeMedica,
            UbsEspecialidadeMedicaKey,
            UbsServicoSaude,
            UbsServicoSaudeKey
        >
    {
        Task<IList<EspecialidadeMedica>> GetAllEspecialidadeMedica();
        Task<IList<ServicoSaude>> GetAllServicoSaude();
        EspecialidadeMedica GetEspecialidadeMedica(int key);

        ServicoSaude GetServicoSaude(int key);

        [TransactionRequired]
        Task AlterarAsync(UnidadeBasicaSaude entity, ISet<UbsEspecialidadeMedica> detail1, ISet<UbsServicoSaude> detail2, Delegate upload);

    }
}