using System;
using System.Linq.Expressions;
using CidConnectada.Dao.Comunicacao;
using CidConnectada.Entities.Model.Comunicacao;
using CidConnectada.Services.Intf.Comunicacao;
using Zenite.Pi.Context;
using Zenite.Pi.Services.Impl;

namespace CidConnectada.Services.Impl.Comunicacao
{
    public class PesquisaService : CadastroBaseService<Pesquisa, PesquisaDao, int, int, string>, IPesquisaService
    {
        public PesquisaService(
            PesquisaDao _cadDao,
            Func<ContextRequest<int, string>> contextFactory
        ) 
          : base(_cadDao, contextFactory)
        {
        }
        #region CRUD

        public override string GetNomeEntidade(int indexDetail = 0)
        {
            return "Pesquisa";
        }

        public override object GetValorCampoDescritivoPadrao(Pesquisa entity)
        {
            return $"{entity.Nome}";
        }

        protected override Expression<Func<Pesquisa, bool>> GetUnicidadeFilter(Pesquisa entity)
        {
            return e => e.Nome == entity.Nome && e.Key != entity.Key;
        }

        #endregion
    }
}
