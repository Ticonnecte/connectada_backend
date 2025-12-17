using CidConnectada.Entities.Model.Noticias;
using CidConnectada.Services.Intf.Infos;
using CidConnectada.Services.Intf.Noticias;
using CidConnectada.Webapi.Models.Noticias;
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

namespace CidConnectada.Webapi.Controllers.Noticias
{
    [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
    [RoutePrefix("api/NoticiaCategoria")]
    //[System.Web.Http.Cors.EnableCors("*", "*", "*")]
    public class NoticiaCategoriaController : BaseWebApiController<NoticiaCategoria, NoticiaCategoriaDto,
        INoticiaCategoriaService, int, int, string>
    {
        public NoticiaCategoriaController(INoticiaCategoriaService cadService, AutoMapper.IMapper mapper, Func<ContextRequest<int, string>> contextFactory)
            : base(cadService, mapper, contextFactory)
        {
            GeneroEntidade = GenreEnum.Female;
            Title = "NoticiaCategoria";
        }

        #region CRUD

        [HttpPost]
        [Route("Post")]
        public override async Task<IHttpActionResult> Post(NoticiaCategoriaDto model)
        {
            return await base.Post(model);
        }

        [HttpGet]
        [Route("GetOne")]
        [ResponseType(typeof(NoticiaCategoriaDto))]
        public override async Task<IHttpActionResult> GetOne(int id)
        {
            return await base.GetOne<NoticiaCategoriaDto>(id);
        }

        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IList<NoticiaCategoriaDto>))]
        public override async Task<IHttpActionResult> GetAll()
        {
            return await base.GetAll();
        }

        [HttpPut]
        [ClaimsAuthorize]
        [Route("Put")]
        public override async Task<IHttpActionResult> Put(NoticiaCategoriaDto model)
        {
            return await base.Put<NoticiaCategoriaDto, NoticiaCategoria>(model);
        }

        [HttpDelete]
        [Route("Delete")]
        public override async Task<IHttpActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }

        [HttpGet]
        [Route("GetPage")]
        [ResponseType(typeof(SearchResultDto<NoticiaCategoriaDto>))]
        public override async Task<IHttpActionResult> GetPage([FromUri] SearchOptions options)
        {
            return Ok(await base.GetPageGeneric<NoticiaCategoriaDto>(options));
        }

        [HttpGet]
        [Route("GetFiltered")]
        [ResponseType(typeof(SearchResultDto<NoticiaCategoriaDto>))]
        public override async Task<IHttpActionResult> GetFiltered([FromUri] ContainsFilter filter)
        {
            return Ok(await base.GetFilteredGeneric<NoticiaCategoriaDto>(filter));
        }

        #endregion
    }
}