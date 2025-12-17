using CidConnectada.Dao.Relacionamento;
using CidConnectada.Dao.Saude;
using CidConnectada.Entities.Model.Banners;
using CidConnectada.Entities.Model.Organograma;
using CidConnectada.Entities.Model.Saude;
using CidConnectada.Services.Intf.AWS;
using CidConnectada.Services.Intf.Organograma;
using CidConnectada.Services.Intf.Saude;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Zenite.Pi.Context;
using Zenite.Pi.Entities.Model.Search;
using Zenite.Pi.Exceptions;
using Zenite.Pi.Services.Impl;
using Zenite.Pi.Util.Pagination;

namespace CidConnectada.Services.Impl.Saude
{
    public class UnidadeBasicaSaudeService : CadastroMasterBaseService<
        UnidadeBasicaSaude,
        UnidadeBasicaSaudeDao,
        string,
        UbsEspecialidadeMedica,
        UbsEspecialidadeMedicaKey,
        UbsEspecialidadeMedicaDao,
        UbsServicoSaude,
        UbsServicoSaudeKey,
        UbsServicoSaudeDao,
        int, string>, IUnidadeBasicaSaudeService
  {

    public UnidadeBasicaSaudeService(
        UnidadeBasicaSaudeDao cadDao,
        Func<ContextRequest<int, string>> contextFactory,
        UbsEspecialidadeMedicaDao cadDaoDetail1,
        UbsServicoSaudeDao cadDaoDetail2,
        ServicoSaudeDao servicoSaudeDao,
        EspecialidadeMedicaDao especialidadeMedicaDao,
        IPrefeituraService prefeituraService
        )
      : base(cadDao, contextFactory, cadDaoDetail1, cadDaoDetail2)
    {
      ServicoSaudeDao = servicoSaudeDao;
      EspecialidadeMedicaDao = especialidadeMedicaDao;
      PrefeituraService = prefeituraService;
    }

    #region Dao e Service

    protected readonly ServicoSaudeDao ServicoSaudeDao;
    protected readonly EspecialidadeMedicaDao EspecialidadeMedicaDao;
    protected readonly IPrefeituraService PrefeituraService;

    #endregion

    #region CRUD

    public override string GetNomeEntidade(int indexDetail = 0)
    {
      return "Unidade Básica de Saúde";
    }

    public override object GetValorCampoDescritivoPadrao(UnidadeBasicaSaude entity)
    {
            return $"Nome: {entity.Nome}, CNES: {entity.CodigoCNES}";
        }

        protected override Expression<Func<UnidadeBasicaSaude, bool>> GetUnicidadeFilter(UnidadeBasicaSaude entity)
        {
            return e => e.CodigoCNES == entity.CodigoCNES && e.Key != entity.Key;
        }

        #endregion

    #region Custom

    public async Task<UnidadeBasicaSaude> IncluirAsync(UnidadeBasicaSaude entity, Delegate upload)
    {
        return await IncluirAsync(entity);
    }

    public async Task DeleteAsync(UnidadeBasicaSaude entity, Delegate deleteS3)
    {
        await ExcluirAsync(entity);
    }

    public async Task<IList<EspecialidadeMedica>> GetAllEspecialidadeMedica()
    {
        return await EspecialidadeMedicaDao.AllAsync();
    }

    public async Task<IList<ServicoSaude>> GetAllServicoSaude()
    {
        return await ServicoSaudeDao.AllAsync();
    }

    public EspecialidadeMedica GetEspecialidadeMedica(int key)
    {
        return EspecialidadeMedicaDao.FindByKey(key);
    }

    public ServicoSaude GetServicoSaude(int key)
    {
        return ServicoSaudeDao.FindByKey(key);
    }
    public override Task<bool> CanDeleteAsync(UnidadeBasicaSaude entity)
    {
        IList<string> msgList = new List<string>();
        bool result = entity.UbsEspecialidadeMedicaSet.Any();
        if (result)
        {
            msgList.Add("Operação abortada. A UBS não pode ser deletada pois há Especialidade(s) Médica(s) associada(s) a ela.");
        }
        if (entity.UbsServicoSaudeSet.Any())
        {
            result = true;
            msgList.Add("Operação abortada. A Secretaria não pode ser deletada pois há Serviço(s) de Saúde associado(s) a ela.");
        }
        if (result)
        {
            throw new PiBusinessException(msgList);
        }
        return base.CanDeleteAsync(entity);
    }

        protected override bool MustCascade(int indexDetail)
        {
            return true;
        }

        protected override bool RecordIsRequired(int indexDetail)
        {
            return false;
        }

        public async Task AlterarAsync(UnidadeBasicaSaude entity, ISet<UbsEspecialidadeMedica> detail1, ISet<UbsServicoSaude> detail2, Delegate upload)
        {
            await base.AlterarAsync(entity, detail1, detail2);
        }

        public Task AlterarAsync(UnidadeBasicaSaude entity, Delegate upload)
        {
            throw new NotImplementedException();
        }


        #endregion

    }
}
