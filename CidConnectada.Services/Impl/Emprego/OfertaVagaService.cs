using CidConnectada.Dao.Emprego;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Emprego;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Services.Intf.Emprego;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Zenite.Pi.Context;
using Zenite.Pi.Exceptions;
using Zenite.Pi.Services.Impl;

namespace CidConnectada.Services.Impl.Emprego
{
    public class OfertaVagaService : CadastroBaseService<OfertaVaga, OfertaVagaDao, long, int, string>, IOfertaVagaService
    {
        public OfertaVagaService(OfertaVagaDao _cadDao,
            Func<ContextRequest<int, string>> contextFactory,
            CompetenciaDao competenciaDao,
            HabilidadeDao habilidadeDao,
            FuncaoDao funcaoDao,
            SetorMercadoDao setorMercadoDao,
            FaixaSalarialDao faixaSalarialDao
        )
          : base(_cadDao, contextFactory)
        {
            CompetenciaDao = competenciaDao;
            HabilidadeDao = habilidadeDao;
            FuncaoDao = funcaoDao;
            SetorMercadoDao = setorMercadoDao;
            FaixaSalarialDao = faixaSalarialDao;
        }
        #region Daos
        private readonly CompetenciaDao CompetenciaDao;
        private readonly HabilidadeDao HabilidadeDao;
        private readonly FuncaoDao FuncaoDao;
        private readonly SetorMercadoDao SetorMercadoDao;
        private readonly FaixaSalarialDao FaixaSalarialDao;

        #endregion

        #region CRUD

        public override string GetNomeEntidade(int indexDetail = 0)
        {
            return "Oferta de Vaga";
        }

        public override object GetValorCampoDescritivoPadrao(OfertaVaga entity)
        {
            return $"Funcao: {entity.Funcao.Nome} | Empresa: {entity.NomeEmpresa}";
        }

        protected override Expression<Func<OfertaVaga, bool>> GetUnicidadeFilter(OfertaVaga entity)
        {
            return e => false;
        }

        #endregion

        #region Custom

        protected override string[] DeleteIncludes => new string[1] { "Empregador" };

        public override async Task<bool> IsValidAsync(OfertaVaga entity, bool validateAllProperties = true)
        {
            bool result = true;
            if (((Usuario)Context.User).Key != entity.Empregador.Key && !Context.IsAdmin)
            {
                Context.AddExceptionMessage("Operação Abortada: apenas Administradores podem alterar a Oferta de Vaga(s) de outro cidadão.");
                result = false;
            }
            return result && await base.IsValidAsync(entity, validateAllProperties);
        }

        public override async Task<bool> CanDeleteAsync(OfertaVaga entity)
        {
            if (((Usuario)Context.User).Key != entity.Empregador.Key && !Context.IsAdmin)
            {
                throw new PiBusinessException("Operação Abortada: apenas Administradores podem deletar a Oferta de Vaga(s) de outro cidadão.");
            }
            return true;
        }

        public async Task<IList<string>> SugerirDetalheEmprego(string termo, int limite, TipoEmpregoDetailEnum tipoEmpregoDetail)
        {
            IList<string> result = new List<string>();
            switch (tipoEmpregoDetail)
            {
                case TipoEmpregoDetailEnum.Habilidade:
                    result = await HabilidadeDao.FuzzySearch(termo, limite);
                    break;
                case TipoEmpregoDetailEnum.Competencia:
                    result = await CompetenciaDao.FuzzySearch(termo, limite);
                    break;
                case TipoEmpregoDetailEnum.Funcao:
                    result = await FuncaoDao.FuzzySearch(termo, limite);
                    break;
                case TipoEmpregoDetailEnum.SetorMercado:
                    result = await SetorMercadoDao.FuzzySearch(termo, limite);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tipoEmpregoDetail), tipoEmpregoDetail, null);
            }

            return result;
        }

        public TDetail GetTDetailByName<TDetail>(string nome)
            where TDetail : EmpregoDetail
        {
            if (String.IsNullOrWhiteSpace(nome))
                return null;

            TDetail result = null;
            string detailName = typeof(TDetail).Name;
            switch (detailName)
            {
                case nameof(Competencia):
                    result = (TDetail)(object)CompetenciaDao.SingleOrDefault(n => n.Nome == nome);
                    result = result ?? (TDetail)(object)CompetenciaDao.Add(new Competencia { Nome = nome });
                    break;
                case nameof(Habilidade):
                    result = (TDetail)(object)HabilidadeDao.SingleOrDefault(n => n.Nome == nome);
                    result = result ?? (TDetail)(object)HabilidadeDao.Add(new Habilidade { Nome = nome });
                    break;
                case nameof(Funcao):
                    result = (TDetail)(object)FuncaoDao.SingleOrDefault(n => n.Nome == nome);
                    result = result ?? (TDetail)(object)FuncaoDao.Add(new Funcao { Nome = nome });
                    break;
                case nameof(SetorMercado):
                    result = (TDetail)(object)SetorMercadoDao.SingleOrDefault(n => n.Nome == nome);
                    result = result ?? (TDetail)(object)SetorMercadoDao.Add(new SetorMercado { Nome = nome });
                    break;
                default:
                    throw new ArgumentException($"Tipo não suportado: {typeof(TDetail).Name}");
            }

            return result;
        }

        public async Task<IList<FaixaSalarial>> GetAllFaixaSalarial()
        {
            return await FaixaSalarialDao.AllAsync();
        }

        public FaixaSalarial GetFaixaSalarial(int id)
        {
            return FaixaSalarialDao.FindByKey(id);
        }

        public async Task<IList<OfertaVaga>> GetMinhasVagas()
        {
            int userKey = ((Usuario)Context.User).Key;
            return await cadDao.Where(v => v.Empregador.Key == userKey).ToListAsync();
        }

        #endregion
    }
}
