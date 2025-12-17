using CidConnectada.Dao.Relacionamento;
using CidConnectada.Entities.Model.Relacionamento;
using CidConnectada.Services.Intf.Relacionamento;
using System;
using System.Linq.Expressions;
using Zenite.Pi.Context;
using Zenite.Pi.Services.Impl;

namespace CidConnectada.Services.Impl.Relacionamento
{
    public class DialogoPreConfigService : CadastroBaseService<DialogoPreConfig, DialogoPreConfigDao, int, int, string>, IDialogoPreConfigService
    {
        public DialogoPreConfigService(DialogoPreConfigDao cadDao, Func<ContextRequest<int, string>> contextFactory)
            : base(cadDao, contextFactory) { }


        #region CRUD

        public override string GetNomeEntidade(int indexDetail = 0)
        {
            return "Pré-Configuração de Diálogo";
        }

        public override object GetValorCampoDescritivoPadrao(DialogoPreConfig entity)
        {
            return $"{entity.Nome}";
        }

        protected override Expression<Func<DialogoPreConfig, bool>> GetUnicidadeFilter(DialogoPreConfig entity)
        {
            return e => e.Nome == entity.Nome && e.Key != entity.Key;
        }

        #endregion
    }
}
