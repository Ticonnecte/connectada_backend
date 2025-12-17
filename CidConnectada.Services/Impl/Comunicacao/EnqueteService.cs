using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CidConnectada.Dao.Comunicacao;
using CidConnectada.Entities.Model.Comunicacao;
using CidConnectada.Entities.Model.Dto;
using CidConnectada.Services.Intf.Comunicacao;
using CidConnectada.Services.Intf.Organograma;
using Zenite.Pi.Context;
using Zenite.Pi.Services.Impl;

namespace CidConnectada.Services.Impl.Comunicacao
{
    public class EnqueteService : CadastroMasterBaseService<Enquete, EnqueteDao, int, EnqueteOpcao, EnqueteOpcaoKey, EnqueteOpcaoDao, int, string>, IEnqueteService
    {
        public EnqueteService(
            EnqueteDao dao,
            Func<ContextRequest<int, string>> contextFactory, 
            EnqueteOpcaoDao servDaoDetail1,
            EnqueteRespostaDao enqueteRespostaDao
        ) 
          : base(dao, contextFactory, servDaoDetail1)
        {
          EnqueteRespostaDao = enqueteRespostaDao;
        }

        #region Daos

        private readonly EnqueteRespostaDao EnqueteRespostaDao;

        #endregion

        #region CRUD

        public override string GetNomeEntidade(int indexDetail = 0)
        {
            return "Enquete";
        }

        public override object GetValorCampoDescritivoPadrao(Enquete entity)
        {
            return entity.Nome;
        }

        protected override Expression<Func<Enquete, bool>> GetUnicidadeFilter(Enquete entity)
        {
            return e => e.Nome == entity.Nome
                && e.Key != entity.Key;
        }

        protected override bool MustCascade(int indexDetail)
        {
            return true;
        }

        protected override bool RecordIsRequired(int indexDetail)
        {
            return true;
        }

        protected async override Task<bool> CanDeleteDynamic(Enquete entity, bool isAsync = false)
        {
            bool result = true;
            if (entity.EnqueteOpcaoSet.Any(o => o.EnqueteRespostaSet.Any()))
            {
                Context.AddExceptionMessage("Operação abordata.\nEsta eqnuate ja possui resposta(s) associada(s)");
                result = false;
            }
            return result;
        }

        #endregion

        #region Custom

        public IList<EnqueteResposta> IncluirEnqueteResposta(IList<EnqueteResposta> respostas)
        {
            IList<EnqueteResposta> result = new List<EnqueteResposta>();
            foreach (var item in respostas)
            {
                EnqueteResposta resposta = EnqueteRespostaDao.Add(item);
                result.Add(resposta);
            }

            return respostas;
        }

        public IList<EnqueteResposta> GetRespostasDoUsuario(int enqueteId, int usuarioId)
        {
            return EnqueteRespostaDao.Where(r => r.EnqueteOpcao.EnqueteId == enqueteId && r.Usuario.Key == usuarioId).ToList();
        }

        public async Task<bool> EstaRespondida(int enqueteId, int usuarioId)
        {
            return await EnqueteRespostaDao.AnyAsync(r => r.EnqueteOpcao.EnqueteId == enqueteId && r.Usuario.Key == usuarioId);
        }

        public async Task<EnqueteResultadoDto> GetResultado(int enqueteId)
        {
            Enquete enquete = await cadDao.FindByKeyPlusAsync(cadDao.DefaultIncludes, enqueteId);
            int totalVotos = await EnqueteRespostaDao.CountAsync(r => r.EnqueteOpcao.EnqueteId == enqueteId);

            EnqueteResultadoDto result = new EnqueteResultadoDto
            {
                key = enquete.Key,
                nome = enquete.Nome,
                totalVotos = totalVotos
            };

            foreach (var opcao in enquete.EnqueteOpcaoSet)
            {
                int qtdeVotos = await EnqueteRespostaDao.CountAsync(r => r.EnqueteOpcao.EnqueteId == enqueteId && r.EnqueteOpcao.OpcaoIdx == opcao.OpcaoIdx);

                var opcaoResultado = new EnqueteOpcaoResultadoDto
                {
                    opcaoIdx = opcao.OpcaoIdx,
                    texto = opcao.Texto,
                    qtdeVotos = qtdeVotos
                };

                result.resultado.Add(opcaoResultado);
            }

            return result;
        }

        #endregion
    }
}
