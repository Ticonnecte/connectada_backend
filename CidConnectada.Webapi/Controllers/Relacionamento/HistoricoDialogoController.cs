using CidConnectada.Entities.Filter;
using CidConnectada.Entities.Model.Relacionamento;
using CidConnectada.Services.Intf.Organograma;
using CidConnectada.Services.Intf.Relacionamento;
using CidConnectada.Webapi.Models.Organograma;
using CidConnectada.Webapi.Models.Relacionamento;
using CidConnectada.Website.Filters;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Zenite.Pi.Context;
using Zenite.Pi.Entities.Enums;
using Zenite.Pi.Entities.Model.Search;
using Zenite.Pi.Web.Models.Cadastro;
using Zenite.Pi.Web.Models.Pesquisa;
using Zenite.Pi.Web.WebApi;

namespace CidConnectada.Webapi.Controllers.Relacionamento
{
    [RoutePrefix("api/HistoricoDialogo")]
    [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
    public class HistoricoDialogoController : BaseWebApiController<HistoricoDialogo, HistoricoDialogoDto, IHistoricoDialogoService, HistoricoDialogoKey, int, string>
    {
        public HistoricoDialogoController(IHistoricoDialogoService cadService, AutoMapper.IMapper mapper, Func<ContextRequest<int, string>> contextFactory)
            : base(cadService, mapper, contextFactory)
        {
            GeneroEntidade = GenreEnum.Male;
            Title = "Histórico de Diálogos";
        }

        #region CRUD

        protected override object GetPostResult<TDto>(HistoricoDialogo entity)
        {
            return AMapper.Map<HistoricoDialogoViewDto>(entity);
        }

        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(HistoricoDialogoDto))]
        public override async Task<IHttpActionResult> Post(HistoricoDialogoDto model)
        {
            return await base.Post(model);
        }

        [HttpGet]
        [Route("GetOne")]
        [ResponseType(typeof(HistoricoDialogoViewDto))]
        public override async Task<IHttpActionResult> GetOne(HistoricoDialogoKey id)
        {
            return await base.GetOne<HistoricoDialogoViewDto>(id);
        }

        [HttpDelete]
        [Route("Delete")]
        public override async Task<IHttpActionResult> Delete([FromUri] HistoricoDialogoKey id)
        {
            return await base.Delete(id);
        }

        [HttpGet]
        [Route("GetFiltered")]
        [ResponseType(typeof(SearchResultDto<HistoricoDialogoFullViewDto>))]
        public override async Task<IHttpActionResult> GetFiltered([FromUri] ContainsFilter filter)
        {
            return Ok(await base.GetFilteredGeneric<HistoricoDialogoFullViewDto>(filter));
        }

        [HttpGet]
        [Route("GetFilteredByDialogo")]
        [ResponseType(typeof(SearchResultDto<HistoricoDialogoFullViewDto>))]
        public async Task<IHttpActionResult> GetFilteredByDialogo([FromUri] HistDialogoContainsFilter filter)
        {
            Context.CacheRequest.Add("DialogoKey", filter.dialogoKey);
            return Ok(await base.GetFilteredGeneric<HistoricoDialogoFullViewDto>(filter));
        }

        #endregion
    }
}