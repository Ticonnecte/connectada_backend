using CidConnectada.Entities.Model.Comunicacao;
using CidConnectada.Services.Intf.Comunicacao;
using CidConnectada.Webapi.Models.Comunicacao;
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

namespace CidConnectada.Webapi.Controllers.Comunicacao
{
    [RoutePrefix("api/Pesquisa")]
    [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO,CIDADAO")]
    public class PesquisaController : BaseWebApiController<Pesquisa, PesquisaDto, IPesquisaService, int, int, string>
    {
        public PesquisaController(IPesquisaService cadService, AutoMapper.IMapper mapper, Func<ContextRequest<int, string>> contextFactory)
            : base(cadService, mapper, contextFactory)
        {
            GeneroEntidade = GenreEnum.Female;
            Title = "Pesquisa";
        }

        #region Custom

        [HttpGet]
        [Route("GetVigentes")]
        [ResponseType(typeof(SearchResultDto<PesquisaDto>))]
        public async Task<IHttpActionResult> GetVigentes([FromUri] ContainsFilter filter)
        {
            Context.CacheRequest.Add("GetVigentes", true);
            return await base.GetFiltered(filter);
        }

        #endregion

        #region CRUD

        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(PesquisaDto))]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public override async Task<IHttpActionResult> Post(PesquisaDto model)
        {
            return await base.Post(model);
        }

        [HttpGet]
        [Route("GetOne")]
        [ResponseType(typeof(PesquisaDto))]
        public override async Task<IHttpActionResult> GetOne(int id)
        {
            return await base.GetOne(id);
        }

        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(PesquisaDto))]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public override async Task<IHttpActionResult> Put(PesquisaDto model)
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
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        [ResponseType(typeof(SearchResultDto<PesquisaDto>))]
        public override async Task<IHttpActionResult> GetFiltered([FromUri] ContainsFilter filter)
        {
            return await base.GetFiltered(filter);
        }

        #endregion
    }
}