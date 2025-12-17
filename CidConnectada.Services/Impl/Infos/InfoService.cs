using Amazon.IdentityManagement.Model;
using CidConnectada.Dao.Infos;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Banners;
using CidConnectada.Entities.Model.Infos;
using CidConnectada.Services.Intf.AWS;
using CidConnectada.Services.Intf.Infos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Zenite.Pi.Context;
using Zenite.Pi.Services.Impl;

namespace CidConnectada.Services.Impl.Infos
{
    public class InfoService : CadastroMasterBaseService<Info, InfoDao, string, InfoImages, HtmlImagesKey, InfoImagesDao, int, string>, IInfoService
    {
        public InfoService(
            InfoDao _cadDao,
            Func<ContextRequest<int, string>> contextFactory,
            InfoImagesDao detailDao
        ) 
            : base(_cadDao, contextFactory, detailDao)
        {
        }

        #region CRUD
        public override string GetNomeEntidade(int indexDetail = 0)
        {
            return "Lead";
        }

        public override object GetValorCampoDescritivoPadrao(Info entity)
        {
            return entity.Lead;
        }

        protected override Expression<Func<Info, bool>> GetUnicidadeFilter(Info entity)
        {
            return e => e.Lead == entity.Lead && e.Key != entity.Key;
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

        #region CUSTOM

        protected override async Task ExcluirDynamic(Info entity, bool async = false)
        {
            //await DeleteS3Images(entity);
            await base.ExcluirDynamic(entity, async);
        }

        public IList<Info> GetAtivasByCategoria(int categoriaKey)
        {
            return cadDao.Where(c => c.Ativa.HasValue && c.Ativa.Value && c.Categoria.Key == categoriaKey).ToList();
        }

        public async Task<Info> IncluirAsync(Info entity, Delegate upload)
        {
            return await IncluirAsync(entity);
        }

        public async Task AlterarAsync(Info entity, ISet<InfoImages> listEntitiesDetail1, Delegate upload)
        {
            await AlterarAsync(entity, listEntitiesDetail1);
        }

        public async Task DeleteAsync(Info entity, Delegate deleteS3)
        {
            await ExcluirAsync(entity);
        }


        #endregion
    }
}
