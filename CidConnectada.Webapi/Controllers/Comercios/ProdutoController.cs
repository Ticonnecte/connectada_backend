using CidConnectada.Entities.Filter;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Banners;
using CidConnectada.Entities.Model.Comercios;
using CidConnectada.Services.Impl.AWS;
using CidConnectada.Services.Intf.AWS;
using CidConnectada.Services.Intf.Comercios;
using CidConnectada.Webapi.Models.Comercios;
using CidConnectada.Website.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Zenite.Pi.Context;
using Zenite.Pi.Entities.Enums;
using Zenite.Pi.Web.Models.Cadastro;
using Zenite.Pi.Web.Models.Pesquisa;
using Zenite.Pi.Web.WebApi;

namespace CidConnectada.Webapi.Controllers.Comercios
{
    //[ClaimsAuthorize]
    [RoutePrefix("api/Produto")]
    public class ProdutoController : BaseWebApiController<Produto, ProdutoDto, IProdutoService, string, int, string>
    {
        public ProdutoController(
            IProdutoService cadService,
            AutoMapper.IMapper mapper,
            Func<ContextRequest<int, string>> contextFactory,
            IAWSS3Service aWSS3Service
        )
            : base(cadService, mapper, contextFactory)
        {
            GeneroEntidade = GenreEnum.Male;
            Title = "Produto";
            AWSS3Service = aWSS3Service;
        }

        #region Services

        private readonly IAWSS3Service AWSS3Service;

        #endregion

        #region CRUD

        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(ProdutoDto))]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO,CIDADAO")]
        public override async Task<IHttpActionResult> Post(ProdutoDto model)
        {
            return await base.Post(model);
        }

        [HttpGet]
        [Route("GetOne")]
        [ResponseType(typeof(ProdutoDto))]
        public override async Task<IHttpActionResult> GetOne(string id)
        {
            return await base.GetOne(id);
        }

        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IList<ProdutoDto>))]
        public override async Task<IHttpActionResult> GetAll()
        {
            return await base.GetAll();
        }

        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(ProdutoDto))]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO,CIDADAO")]
        public override async Task<IHttpActionResult> Put(ProdutoDto model)
        {
            return await base.Put(model);
        }

        [HttpGet]
        [Route("GetFiltered")]
        [ResponseType(typeof(SearchResultDto<ProdutoDto>))]
        public async Task<IHttpActionResult> GetFiltered([FromUri] ProdutoFilter filter)
        {
            Context.CacheRequest.Add("comercioId", filter.comercioId);
            return await base.GetFiltered(filter);
        }

        #endregion

        #region Custom

        protected async override Task IncluirAsync(Produto entity)
        {
            Delegate upload = new Func<object, Task>(async (ent) => await AWSS3Service.UploadAsync((Produto)ent));
            await cadService.IncluirAsync(entity, upload);
        }

        protected async override Task AlterarAsync(Produto entity)
        {
            Delegate upload = new Func<object, Task>(async (ent) => await AWSS3Service.UploadAsync((Produto)ent));
            await cadService.AlterarAsync(entity, upload);
        }

        [HttpDelete]
        [Route("Delete")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO,CIDADAO")]
        public override async Task<IHttpActionResult> Delete(string id)
        {
            Produto entity = await cadService.ObterAsync(id);
            Delegate deleteS3 = new Func<object, Task>(async (ent) => await AWSS3Service.DeleteAsync((Produto)ent));
            await cadService.DeleteAsync(entity, deleteS3);

            return Ok();
        }


        #endregion

    }
}
