using CidConnectada.Dao.Comercios;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Banners;
using CidConnectada.Entities.Model.Comercios;
using CidConnectada.Services.Intf.AWS;
using CidConnectada.Services.Intf.Comercios;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Zenite.Pi.Context;
using Zenite.Pi.Services.Impl;

namespace CidConnectada.Services.Impl.Comercios
{
    public class ProdutoService : CadastroBaseService<Produto, ProdutoDao, string, int, string>, IProdutoService
    {
        public ProdutoService(ProdutoDao _cadDao, Func<ContextRequest<int, string>> contextFactory)
            : base(_cadDao, contextFactory)
        {
        }
        #region Daos-Services

        #endregion

        #region CRUD
        public override string GetNomeEntidade(int indexDetail = 0)
        {
            return "Produto";
        }

        public override object GetValorCampoDescritivoPadrao(Produto entity)
        {
            return entity.Nome;
        }

        protected override Expression<Func<Produto, bool>> GetUnicidadeFilter(Produto entity)
        {
            return e => e.Nome == entity.Nome && e.Comercio.Key == entity.Comercio.Key
                && e.Key != entity.Key;
        }

        #endregion

        #region Custom

        protected override async Task<bool> IsValidDynamic(Produto entity, bool isAsync, bool validateAllProperties = true)
        {
            return await base.IsValidDynamic(entity, isAsync, validateAllProperties) && CheckUser(entity); ;
        }

        protected override async Task<bool> CanDeleteDynamic(Produto entity, bool isAsync = false)
        {
            return await base.CanDeleteDynamic(entity, isAsync) && CheckUser(entity);
        }

        protected bool CheckUser(Produto entity)
        {
            bool result = true;
            if (((Usuario)Context.User).Key != entity.Comercio.Cidadao.Key)
            {
                Context.AddExceptionMessage("Operação Abortada: Não é possivel alterar o comércio de outro cidadão.");
                result = false;
            }
            return result;
        }

        public async Task<Produto> IncluirAsync(Produto entity, Delegate upload)
        {
            return await IncluirAsync(entity);
        }

        public async Task AlterarAsync(Produto entity, Delegate upload)
        {
            await AlterarAsync(entity);
        }

        public async Task DeleteAsync(Produto entity, Delegate deleteS3)
        {
            await ExcluirAsync(entity);
        }

        #endregion
    }
}
