using CidConnectada.Dao.Comercios;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Comercios;
using CidConnectada.Services.Intf.AWS;
using CidConnectada.Services.Intf.Comercios;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Zenite.Pi.Context;
using Zenite.Pi.Exceptions;
using Zenite.Pi.Services.Impl;

namespace CidConnectada.Services.Impl.Comercios
{
    public class ComercioService : CadastroMasterBaseService<Comercio, ComercioDao, string, ComercioCategoriaVinculo, ComercioCategoriaVinculoKey, ComercioCategoriaVinculoDao, int, string>, IComercioService
    {
        public ComercioService(ComercioDao dao, Func<ContextRequest<int, string>> contextFactory, ComercioCategoriaVinculoDao servDaoDetail1) : base(dao, contextFactory, servDaoDetail1)
        {
        }

        #region CRUD

        public override string GetNomeEntidade(int indexDetail = 0)
        {
            return "Comércio";
        }

        public override object GetValorCampoDescritivoPadrao(Comercio entity)
        {
            return entity.Nome;
        }

        protected override Expression<Func<Comercio, bool>> GetUnicidadeFilter(Comercio entity)
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
            return false;
        }

        #endregion

        #region Custom

        protected override async Task<bool> IsValidDynamic(Comercio entity, bool isAsync, bool validateAllProperties = true)
        {
            return await base.IsValidDynamic(entity, isAsync, validateAllProperties) && CheckUser(entity); ;
        }

        protected override async Task<bool> CanDeleteDynamic(Comercio entity, bool isAsync = false)
        {
            return await base.CanDeleteDynamic(entity, isAsync) && CheckUser(entity);
        }

        protected bool CheckUser(Comercio entity)
        {
            bool result = true;
            if (((Usuario)Context.User).Key != entity.Cidadao.Key && !Context.IsAdmin)
            {
                throw new PiBusinessException("Operação Abortada: apenas Administradores podem alterar/deletar o Comércio de outro cidadão.");
            }
            return result;
        }

        public async Task<IList<Comercio>> GetByTipo(int tipo)
        {
            return await cadDao.Where(c => c.TipoComercio.Key == tipo).ToListAsync();
        }

        public async Task<Comercio> IncluirAsync(Comercio entity, Delegate upload)
        {
            return await IncluirAsync(entity);
        }

        public async Task AlterarAsync(Comercio entity, Delegate upload)
        {
            await AlterarAsync(entity);
        }

        public async Task DeleteAsync(Comercio entity, Delegate deleteS3)
        {
            await ExcluirAsync(entity);
        }

        #endregion

    }
}
