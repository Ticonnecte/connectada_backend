using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CidConnectada.Dao.Banners;
using CidConnectada.Entities.Model.Banners;
using CidConnectada.Services.Intf.AWS;
using CidConnectada.Services.Intf.Banners;
using Zenite.Pi.Context;
using Zenite.Pi.Services.Impl;

namespace CidConnectada.Services.Impl.Banners
{
    public class BannerService : CadastroBaseService<Banner, BannerDao, string, int, string>, IBannerService
    {
        public BannerService(
            BannerDao _cadDao,
            Func<ContextRequest<int, string>> contextFactory,
            RotaInternaDao rotaInternaDao
        )
            : base(_cadDao, contextFactory)
        {
            RotaInternaDao = rotaInternaDao;
        }

        #region Daos-Services

        private readonly RotaInternaDao RotaInternaDao;

        #endregion

        #region Custom

        public async Task<IList<RotaInterna>> GetRotasInternasAsync()
        {
            return await RotaInternaDao.Where(r => r.EhBanner).ToListAsync();
        }

        public async Task<IList<Banner>> GetHomeBannersAsync()
        {
            var test = await cadDao.Where(b => b.EstaNaHome).ToListAsync();
            return test;
        }

        public RotaInterna FindRotaById(int linkId)
        {
            return RotaInternaDao.FindByKey(linkId);
        }

        public async Task<Banner> IncluirAsync(Banner entity, Delegate upload)
        {
            return await base.IncluirDynamic(entity, true);
        }

        #endregion

        #region CRUD

        public override string GetNomeEntidade(int indexDetail = 0)
        {
            return "Banner";
        }

        public override object GetValorCampoDescritivoPadrao(Banner entity)
        {
            return $"Nome: {entity.Nome}";
        }

        protected override Expression<Func<Banner, bool>> GetUnicidadeFilter(Banner entity)
        {
            return e => e.Nome == entity.Nome && e.Key != entity.Key;
        }

        public async Task AlterarAsync(Banner entity, Delegate upload)
        {
            await AlterarAsync(entity);
        }

        public async Task DeleteAsync(Banner entity, Delegate deleteS3)
        {
            await ExcluirAsync(entity);
        }

        #endregion
    }
}