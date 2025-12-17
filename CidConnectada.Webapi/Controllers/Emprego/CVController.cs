using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Emprego;
using CidConnectada.Services.Intf.Account;
using CidConnectada.Services.Intf.Comunicacao;
using CidConnectada.Services.Intf.Emprego;
using CidConnectada.Webapi.Models.Emprego;
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

namespace CidConnectada.Webapi.Controllers.Emprego
{
    [RoutePrefix("api/CV")]
    [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO,CIDADAO")]
    public class CVController : MasterDetailWebApiController<CurriculumVitae, CVDto, ICurriculumVitaeService, int,
        CVExperiencia, CVExperienciaDto, CVExperienciaKey, int, string>
    {

        public CVController(ICurriculumVitaeService cadService, AutoMapper.IMapper mapper, Func<ContextRequest<int, string>> contextFactory)
            : base(cadService, mapper, contextFactory)
        {
            GeneroEntidade = GenreEnum.Male;
            Title = "CurriculumVitae";
        }

        #region Custom

        [HttpGet]
        [Route("MeuCurriculo")]
        [ResponseType(typeof(CVDto))]
        public async Task<IHttpActionResult> MeuCurriculo()
        {
            CurriculumVitae entity = await cadService.GetMyCV();
            if (entity == null)
                entity = new CurriculumVitae();

            return Ok(AMapper.Map<CVDto>(entity));
        }

        [HttpPut]
        [Route("Insert")]
        [ResponseType(typeof(CVDto))]
        public async Task<IHttpActionResult> Insert(CVDto model)
        {
            CurriculumVitae entity = await cadService.GetMyCV();
            if (entity == null)
                return await base.Post(model);

            model.key = entity.Key;
            return await base.Put(model);
        }

        #endregion

        #region CRUD

        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(CVDto))]
        public override async Task<IHttpActionResult> Post(CVDto model)
        {
            return await base.Post(model);
        }

        [HttpGet]
        [Route("GetOne")]
        [ResponseType(typeof(CVDto))]
        public override async Task<IHttpActionResult> GetOne(int id)
        {
            return await base.GetOne(id);
        }

        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(CVDto))]
        public override async Task<IHttpActionResult> Put(CVDto model)
        {
            return await base.Put(model);
        }

        [HttpDelete]
        [Route("Delete")]
        public override async Task<IHttpActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }

        [HttpGet]
        [Route("GetPage")]
        [ResponseType(typeof(SearchResultDto<CVViewDto>))]
        public override async Task<IHttpActionResult> GetPage([FromUri] SearchOptions options)
        {
            return Ok(await base.GetPageGeneric<CVViewDto>(options));
        }

        [HttpGet]
        [Route("GetFiltered")]
        [ResponseType(typeof(SearchResultDto<CVViewDto>))]
        public override async Task<IHttpActionResult> GetFiltered([FromUri] ContainsFilter filter)
        {
            return Ok(await base.GetFilteredGeneric<CVViewDto>(filter));
        }

        #endregion
    }
}