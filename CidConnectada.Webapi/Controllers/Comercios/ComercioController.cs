using CidConnectada.Entities.Filter;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Comercios;
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
    [ClaimsAuthorize]
    [RoutePrefix("api/Comercio")]
    public class ComercioController : BaseWebApiController<Comercio, ComercioDto, IComercioService, string, int, string>
    {
        public ComercioController(
            IComercioService cadService,
            AutoMapper.IMapper mapper,
            Func<ContextRequest<int, string>> contextFactory,
            ITipoComercioService tipoComercioService,
            IAWSS3Service awsS3Service
        )
            : base(cadService, mapper, contextFactory)
        {
            TipoComercioService = tipoComercioService;
            AWSS3Service = awsS3Service;
            GeneroEntidade = GenreEnum.Male;
            Title = "Comércio";
        }

        #region Services

        private readonly ITipoComercioService TipoComercioService;

        private readonly IAWSS3Service AWSS3Service;

        #endregion

        #region CRUD

        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(ComercioDto))]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO,CIDADAO")]
        public override async Task<IHttpActionResult> Post(ComercioDto model)
        {
            return await base.Post(model);
        }

        [HttpGet]
        [Route("GetOne")]
        [ResponseType(typeof(ComercioDto))]
        public override async Task<IHttpActionResult> GetOne(string id)
        {
            return await base.GetOne(id);
        }

        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IList<ComercioDto>))]
        public override async Task<IHttpActionResult> GetAll()
        {
            return await base.GetAll();
        }

        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(ComercioDto))]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO,CIDADAO")]
        public override async Task<IHttpActionResult> Put(ComercioDto model)
        {
            //Comercio entity = await cadService.ObterAsync(model.key);
            //if (((Usuario)Context.User).Key != entity.Cidadao.Key)
            //    return BadRequest("Operação Abortada: Não é possivel alterar o comércio de outro cidadão.");
            
            Context.CacheRequest.Add("CategoriaKeys", model.categorias);
            return await base.Put(model);
        }

        [HttpGet]
        [Route("GetFiltered")]
        [ResponseType(typeof(SearchResultDto<ComercioDto>))]
        public async Task<IHttpActionResult> GetFiltered([FromUri] ComercioFilter filter)
        {
            Context.CacheRequest.Add("tipoComercioId", filter.tipoComercioId);
            return await base.GetFiltered(filter);
        }

        #endregion

        #region Custom

        protected async override Task IncluirAsync(Comercio entity)
        {
            Delegate upload = new Func<object, Task>(async (ent) => await AWSS3Service.UploadAsync((Comercio)ent));
            await cadService.IncluirAsync(entity, upload);
        }

        protected async override Task AlterarAsync(Comercio entity)
        {
            Delegate upload = new Func<object, Task>(async (ent) => await AWSS3Service.UploadAsync((Comercio)ent));
            await cadService.AlterarAsync(entity, upload);
        }

        [HttpDelete]
        [Route("Delete")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO,CIDADAO")]
        public override async Task<IHttpActionResult> Delete(string id)
        {
            Comercio entity = await cadService.ObterAsync(id);
            Delegate deleteS3 = new Func<object, Task>(async (ent) => await AWSS3Service.DeleteAsync((Comercio)ent));
            await cadService.DeleteAsync(entity, deleteS3);

            return Ok();
        }

        protected override Action<AutoMapper.IMappingOperationOptions> GetMappingOptions(IDictionary<string, object> pairs)
        {
            pairs.Add("CategoriasFunc", GetCategorias);
            return base.GetMappingOptions(pairs);
        }

        protected Func<IList<int>, Comercio, ISet<ComercioCategoriaVinculo>> GetCategorias => (categoriaKeys, entity) =>
        {
            ISet<ComercioCategoriaVinculo> result = new HashSet<ComercioCategoriaVinculo>();
            foreach (int key in categoriaKeys)
            {
                result.Add(new ComercioCategoriaVinculo()
                {
                    CategoriaId = key,
                    Categoria = TipoComercioService.GetCategoria(key),
                    ComericoId = entity.Key,
                    Comercio = entity
                });
            }
            return result;
        };

        [HttpGet]
        [Route("GetByTipo")]
        [ResponseType(typeof(SearchResultDto<ComercioDto>))]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO,CIDADAO")]
        public async Task<IHttpActionResult> GetByTipo([FromUri] ComercioFilter filter)
        {
            return await GetFiltered(filter);
        }

        [HttpGet]
        [Route("MeuComercio")]
        [ResponseType(typeof(SearchResultDto<ComercioDto>))]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO,CIDADAO")]
        public async Task<IHttpActionResult> MeuComercio([FromUri] ComercioFilter filter)
        {
            int userId = ((Usuario)Context.User).Key;
            Context.CacheRequest.Add("userId", userId);
            return await GetFiltered(filter);
        }

        #endregion
    }
}
