using CidConnectada.Entities.Model.Comercios;
using CidConnectada.Services.Intf.Comercios;
using CidConnectada.Webapi.Models.Comercios;
using CidConnectada.Webapi.Models.Organograma;
using CidConnectada.Website.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Zenite.Pi.Context;
using Zenite.Pi.Entities.Enums;
using Zenite.Pi.Entities.Model.Search;
using Zenite.Pi.Web.Models.Cadastro;
using Zenite.Pi.Web.Models.Pesquisa;
using Zenite.Pi.Web.WebApi;

namespace CidConnectada.Webapi.Controllers.Comercios
{
    [ClaimsAuthorize]
    [RoutePrefix("api/TipoComercio")]
    public class TipoComercioController : MasterDetailWebApiController<TipoComercio, TipoComercioDto, ITipoComercioService, int, CategoriaTipoComercio, CategoriaTipoComercioDto, int, int, string>
    {
        public TipoComercioController(ITipoComercioService cadService, AutoMapper.IMapper mapper, Func<ContextRequest<int, string>> contextFactory)
            : base(cadService, mapper, contextFactory)
        {
            GeneroEntidade = GenreEnum.Male;
            Title = "Tipo Comércio";
        }

        #region CRUD

        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(TipoComercioDto))]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public override async Task<IHttpActionResult> Post(TipoComercioDto model)
        {
            return await base.Post(model);
        }

        [HttpGet]
        [Route("GetOne")]
        [ResponseType(typeof(TipoComercioDto))]
        public override async Task<IHttpActionResult> GetOne(int id)
        {
            return await base.GetOne(id);
        }

        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IList<TipoComercioDto>))]
        public override async Task<IHttpActionResult> GetAll()
        {
            return await base.GetAll();
        }

        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(TipoComercioDto))]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public override async Task<IHttpActionResult> Put(TipoComercioDto model)
        {
            return await base.Put(model);
        }

        [HttpDelete]
        [Route("Delete")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public override async Task<IHttpActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }

        [HttpGet]
        [Route("GetFiltered")]
        [ResponseType(typeof(SearchResultDto<TipoComercioDto>))]
        public override async Task<IHttpActionResult> GetFiltered([FromUri] ContainsFilter filter)
        {
            return await base.GetFiltered(filter);
        }

        #endregion

        #region Custom

        [HttpGet]
        [Route("GetCategorias")]
        [ResponseType(typeof(piLookupModel<int>))]
        public async Task<IHttpActionResult> GetCategorias(int tipoComercioKey)
        {
            return Ok((await cadService.GetCategoriasByTipoAsync(tipoComercioKey)).Select(c => new piLookupModel<int>() { value = c.Key, text = c.Nome, group = c.TipoComercio.Nome }));
        }
        
        [HttpGet]
        [Route("GetHome")]
        [ResponseType(typeof(IList<TipoComercioDto>))]
        public async Task<IHttpActionResult> GetHome(int? qtde = null)
        {
            IList<TipoComercio> tiposComercio = await cadService.GetHome(qtde);
            return Ok(AMapper.Map<IList<TipoComercioDto>>(tiposComercio, opt => opt.Items["Caller"] = $"{nameof(TipoComercioController)}.{nameof(GetHome)}"));
        }
        
        [HttpPut]
        [Route("AlterarOrdemHome")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        [ResponseType(typeof(IList<OrdemHomeDto<int>>))]
        public async Task<IHttpActionResult> AlterarOrdem(IList<OrdemHomeDto<int>> modelList)
        {
            await cadService.AlterarOrdemHome(modelList);
            IList<TipoComercio> tiposComercio = await cadService.GetAllAsync();
            return Ok(AMapper.Map<IList<OrdemHomeDto<int>>>(tiposComercio));
        }

        #endregion
    }
}
