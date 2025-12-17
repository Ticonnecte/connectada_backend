using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Emprego;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Services.Intf.Emprego;
using CidConnectada.Webapi.Models.Emprego;
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

namespace CidConnectada.Webapi.Controllers.Emprego
{
    [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO,CIDADAO")]
    [RoutePrefix("api/OfertaVaga")]
    public class OfertaVagaController : BaseWebApiController<OfertaVaga, OfertaVagaDto, IOfertaVagaService, long, int, string>
    {
        public OfertaVagaController(IOfertaVagaService cadService, AutoMapper.IMapper mapper, Func<ContextRequest<int, string>> contextFactory)
            : base(cadService, mapper, contextFactory)
        {
            GeneroEntidade = GenreEnum.Male;
            Title = "OfertaVaga";
        }

        #region Custom

        [HttpGet]
        [Route("MinhasVagas")]
        [ResponseType(typeof(IList<OfertaVagaBaseDto>))]
        public async Task<IHttpActionResult> GetMinhasVagas()
        {
            IList<OfertaVaga> vagas = await cadService.GetMinhasVagas();
            return Ok(AMapper.Map<IList<OfertaVagaBaseDto>>(vagas));
        }

        [HttpGet]
        [Route("GetAllFaixaSalarial")]
        [ResponseType(typeof(IList<piLookupModel<int>>))]
        public async Task<IHttpActionResult> GetAllFaixaSalarial()
        {
            IList<FaixaSalarial> faixaSalarialList = await cadService.GetAllFaixaSalarial();
            return Ok(AMapper.Map<IList<piLookupModel<int>>>(faixaSalarialList.Select(fs => new piLookupModel<int>
            {
                value = fs.Key,
                text = $"R${fs.ValorMin} à R${fs.ValorMax}"
            })));
        }

        [HttpGet]
        [Route("SugerirHabilidades")]
        [ResponseType(typeof(IList<string>))]
        public async Task<IList<string>> SugerirHabilidades(string termo, int limite)
        {
            return await cadService.SugerirDetalheEmprego(termo, limite, TipoEmpregoDetailEnum.Habilidade);
        }

        [HttpGet]
        [Route("SugerirCompetencias")]
        [ResponseType(typeof(IList<string>))]
        public async Task<IList<string>> SugerirCompetencias(string termo, int limite)
        {
            return await cadService.SugerirDetalheEmprego(termo, limite, TipoEmpregoDetailEnum.Competencia);
        }

        [HttpGet]
        [Route("SugerirFuncoes")]
        [ResponseType(typeof(IList<string>))]
        public async Task<IList<string>> SugerirFuncoes(string termo, int limite)
        {
            return await cadService.SugerirDetalheEmprego(termo, limite, TipoEmpregoDetailEnum.Funcao);
        }

        [HttpGet]
        [Route("SugerirSetores")]
        [ResponseType(typeof(IList<string>))]
        public async Task<IList<string>> SugerirSetores(string termo, int limite)
        {
            return await cadService.SugerirDetalheEmprego(termo, limite, TipoEmpregoDetailEnum.SetorMercado);
        }

        #endregion

        #region CRUD

        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(OfertaVagaDto))]
        public override async Task<IHttpActionResult> Post(OfertaVagaDto model)
        {
            return await base.Post(model);
        }

        [HttpGet]
        [Route("GetOne")]
        [ResponseType(typeof(OfertaVagaDto))]
        public override async Task<IHttpActionResult> GetOne(long id)
        {
            return await base.GetOne(id);
        }

        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(OfertaVagaDto))]
        public override async Task<IHttpActionResult> Put(OfertaVagaDto model)
        {
            return await base.Put(model);
        }

        [HttpDelete]
        [Route("Delete")]
        public override async Task<IHttpActionResult> Delete(long id)
        {
            return await base.Delete(id);
        }

        [HttpGet]
        [Route("GetPage")]
        [ResponseType(typeof(SearchResultDto<OfertaVagaBaseDto>))]
        public override async Task<IHttpActionResult> GetPage([FromUri] SearchOptions options)
        {
            return Ok(await base.GetPageGeneric<OfertaVagaBaseDto>(options));
        }

        [HttpGet]
        [Route("GetFiltered")]
        [ResponseType(typeof(SearchResultDto<OfertaVagaBaseDto>))]
        public override async Task<IHttpActionResult> GetFiltered([FromUri] ContainsFilter filter)
        {
            return Ok(await base.GetFilteredGeneric<OfertaVagaBaseDto>(filter));
        }

        #endregion
    }
}