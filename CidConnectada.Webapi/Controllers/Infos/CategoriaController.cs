using CidConnectada.Entities.Model.Infos;
using CidConnectada.Entities.Model.Noticias;
using CidConnectada.Services.Intf.Emprego;
using CidConnectada.Services.Intf.Infos;
using CidConnectada.Services.Intf.Noticias;
using CidConnectada.Webapi.Models.Infos;
using CidConnectada.Webapi.Models.Noticias;
using CidConnectada.Website.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Zenite.Pi.Context;
using Zenite.Pi.Entities.Enums;
using Zenite.Pi.Entities.Model.Search;
using Zenite.Pi.Util.Pagination;
using Zenite.Pi.Web.Models.Cadastro;
using Zenite.Pi.Web.Models.Pesquisa;
using Zenite.Pi.Web.WebApi;

namespace CidConnectada.Webapi.Controllers.Infos
{
   
    [RoutePrefix("api/Categoria")]
    [ClaimsAuthorize]
    public class CategoriaController : BaseWebApiController<Categoria, CategoriaDto,
        ICategoriaService, int, int, string>
    {
        public CategoriaController(ICategoriaService cadService, AutoMapper.IMapper mapper, Func<ContextRequest<int, string>> contextFactory)
            : base(cadService, mapper, contextFactory)
        {
            GeneroEntidade = GenreEnum.Female;
            Title = "Categoria";
        }

        #region CRUD

        [HttpPost]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        [Route("Post")]
        public override async Task<IHttpActionResult> Post(CategoriaDto model)
        {
            return await base.Post(model);
        }

        [HttpGet]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        [Route("GetOne")]
        [ResponseType(typeof(CategoriaDto))]
        public override async Task<IHttpActionResult> GetOne(int id)
        {
            return await base.GetOne<CategoriaDto>(id);
        }

        [HttpGet]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        [Route("GetAll")]
        [ResponseType(typeof(IList<CategoriaDto>))]
        public override async Task<IHttpActionResult> GetAll()
        {
            return await base.GetAll();
        }

        [HttpPut]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
      
        [Route("Put")]
        public override async Task<IHttpActionResult> Put(CategoriaDto model)
        {
            return await base.Put<CategoriaDto, Categoria>(model);
        }

        [HttpDelete]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        [Route("Delete")]
        public override async Task<IHttpActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }

        [HttpGet]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        [Route("GetPage")]
        [ResponseType(typeof(SearchResultDto<CategoriaDto>))]
        public override async Task<IHttpActionResult> GetPage([FromUri] SearchOptions options)
        {
            return Ok(await base.GetPageGeneric<CategoriaDto>(options));
        }

        [HttpGet]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        [Route("GetFiltered")]
        [ResponseType(typeof(SearchResultDto<CategoriaDto>))]
        public override async Task<IHttpActionResult> GetFiltered([FromUri] ContainsFilter filter)
        {
            PagedResult<Categoria> pagedResult = await cadService.SearchPagedAsync(filter);
            IList<CategoriaDto> modelList = AMapper.Map<IList<CategoriaDto>>(pagedResult.List, GetMappingOptions(GetMapItems("GetFiltered")));
            return Ok(new SearchResultDto<CategoriaDto>
            {
                data = modelList,
                page = pagedResult.CurrentPage,
                pageSize = filter.pageSize,
                totalRows = pagedResult.TotalQuantity
            });

            //return Ok(await base.GetFilteredGeneric<CategoriaDto>(filter));
        }

        #endregion

        #region Custom

        [HttpGet]
        [Route("GetOptions")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        [ResponseType(typeof(IList<piLookupModel<int>>))]
        public async Task<IHttpActionResult> GetOptions()
        {
            return Ok((await cadService.GetAllAsync()).Select(c=>new piLookupModel<int>() { value = c.Key,text = c.Nome, group = c.Ativa.ToString() }).ToList());
        }

        [HttpGet]
        [Route("GetAtivas")]
        [ResponseType(typeof(IList<InfoViewDto>))]
        public async Task<IHttpActionResult> GetAtivas()
        {
            IList<Categoria> list = await cadService.GetAtivasAsync();
            return Ok(AMapper.Map<IList<InfoViewDto>>(list, opt => opt.Items.Add(CALLER_NAME_OF_THE_MAPPER, "InfoCategoria.GetAtivas")));
        }

        #endregion
    }
}
