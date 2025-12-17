using CidConnectada.Dao.Relacionamento;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Entities.Model.Relacionamento;
using CidConnectada.Services.Intf.Relacionamento;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Enums;
using Zenite.Pi.Services.Impl;

namespace CidConnectada.Services.Impl.Relacionamento
{
    public class HistoricoDialogoService : CadastroBaseService<HistoricoDialogo, HistoricoDialogoDao, HistoricoDialogoKey, int, string>, IHistoricoDialogoService
    {

        public HistoricoDialogoService(HistoricoDialogoDao cadDao, Func<ContextRequest<int, string>> contextFactory)
            : base(cadDao, contextFactory) { }

        #region CRUD

        public override string GetNomeEntidade(int indexDetail = 0)
        {
            return "Histórico de Diálogo";
        }

        public override object GetValorCampoDescritivoPadrao(HistoricoDialogo entity)
        {
            return String.Format("{0} mudou o Status para {1}, em {2}", entity.Funcionario.NomeCompleto, Enum.GetName(typeof(DialogoStatusEnum), entity.StatusEnum), entity.DhTransicao);
        }

        protected override Expression<Func<HistoricoDialogo, bool>> GetUnicidadeFilter(HistoricoDialogo entity)
        {
            return hd => hd.Funcionario.Key == entity.Funcionario.Key
                && hd.Dialogo.Key == entity.Dialogo.Key
                && hd.DhTransicao == entity.DhTransicao
                && hd.StatusEnum == entity.StatusEnum
                && hd.SequenciaIndex != entity.SequenciaIndex;
        }

        #endregion

        #region Custom

        public override void SetValoresPadroes(HistoricoDialogo entity, OperacaoEntidadeEnum operacao)
        {
            switch (operacao)
            {
                case OperacaoEntidadeEnum.Incluir:
                    entity.SequenciaIndex = entity.Dialogo.HistoricoDialogoSet.Count() + 1;
                    break;
            }
        }

        protected override Task<HistoricoDialogo> IncluirDynamic(HistoricoDialogo entity, bool async = false)
        {
            entity.Dialogo.DialogoStatusEnum = entity.StatusEnum;
            return base.IncluirDynamic(entity, async);
        }

        #endregion
    }
}
