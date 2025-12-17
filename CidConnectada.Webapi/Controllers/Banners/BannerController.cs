using CidConnectada.Entities.Model.Banners;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Services.Intf.AWS;
using CidConnectada.Services.Intf.Banners;
using CidConnectada.Webapi.Models.Banners;
using CidConnectada.Website.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Zenite.Pi.Context;
using Zenite.Pi.Entities.Enums;
using Zenite.Pi.Entities.Model.Search;
using Zenite.Pi.Web.Models.Cadastro;
using Zenite.Pi.Web.Models.Pesquisa;
using Zenite.Pi.Web.WebApi;

namespace CidConnectada.Webapi.Controllers.Banners
{
    [ClaimsAuthorize]
    [RoutePrefix("api/Banner")]
    public class BannerController : BaseWebApiController<Banner, BannerDto, IBannerService, string, int, string>
    {
        private readonly IAWSS3Service AWSS3Service;

        public BannerController(
            IBannerService cadService,
            AutoMapper.IMapper mapper,
            Func<ContextRequest<int, string>> contextFactory,
            IAWSS3Service awsS3Service
        )
            : base(cadService, mapper, contextFactory)
        {
            AWSS3Service = awsS3Service;
            GeneroEntidade = GenreEnum.Male;
            Title = "Banner";
        }

        #region CRUD

        protected override object GetPutResult<TDto>(Banner entity)
        {
            return AMapper.Map<BannerViewDto>(entity);
        }

        protected override object GetPostResult<TDto>(Banner entity)
        {
            return AMapper.Map<BannerViewDto>(entity);
        }

        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(BannerViewDto))]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public override async Task<IHttpActionResult> Post(BannerDto model)
        {
            return await base.Post(model);
        }

        [HttpGet]
        [Route("GetOne")]
        [ResponseType(typeof(BannerViewDto))]
        public override async Task<IHttpActionResult> GetOne(string id)
        {
            return await base.GetOne<BannerViewDto>(id);
        }

        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(BannerViewDto))]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public override async Task<IHttpActionResult> Put(BannerDto model)
        {
            return await base.Put(model);
        }

        [HttpGet]
        [Route("GetPage")]
        [ResponseType(typeof(SearchResultDto<BannerViewDto>))]
        public override async Task<IHttpActionResult> GetPage([FromUri] SearchOptions options)
        {
            return Ok(await base.GetPageGeneric<BannerViewDto>(options));
        }

        [HttpGet]
        [Route("GetFiltered")]
        [ResponseType(typeof(SearchResultDto<BannerViewDto>))]
        public override async Task<IHttpActionResult> GetFiltered([FromUri] ContainsFilter filter)
        {
            return Ok(await base.GetFilteredGeneric<BannerViewDto>(filter));
        }

        #endregion

        #region Custom

        [HttpGet]
        [Route("GetBannerTipoEnumList")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        [ResponseType(typeof(IList<piLookupModel<int>>))]
        public async Task<IHttpActionResult> GetBannerTipoEnumList()
        {
            return Ok(await Task.Run(() => GetEnum<int, RotaTipoEnum>("BannerTipoEnum")));
        }

        protected async override Task IncluirAsync(Banner entity)
        {
            Delegate upload = new Func<object, Task>(async (ent) => await AWSS3Service.UploadAsync((Banner)ent));
            await cadService.IncluirAsync(entity, upload);
        }

        protected async override Task AlterarAsync(Banner entity)
        {
            Delegate upload = new Func<object, Task>(async (ent) => await AWSS3Service.UploadAsync((Banner)ent));
            await cadService.AlterarAsync(entity, upload);
        }

        [HttpDelete]
        [Route("Delete")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public override async Task<IHttpActionResult> Delete(string id)
        {
            Banner entity = await cadService.ObterAsync(id);
            Delegate deleteS3 = new Func<object, Task>(async (ent) => await AWSS3Service.DeleteAsync((Banner)ent));
            await cadService.DeleteAsync(entity, deleteS3);

            return Ok();
        }

        [HttpGet]
        [Route("GetRotasInternas")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        [ResponseType(typeof(IList<RotaInternaDto>))]
        public async Task<IHttpActionResult> GetRotasInternas()
        {
            return Ok(AMapper.Map<IList<RotaInternaDto>>(await cadService.GetRotasInternasAsync()));
        }

        [HttpGet]
        [Route("GetHome")]
        [ResponseType(typeof(IList<BannerViewDto>))]
        public async Task<IHttpActionResult> GetHome()
        {
            return Ok(AMapper.Map<IList<BannerViewDto>>(await cadService.GetHomeBannersAsync()));
        }

        #endregion
    }
}