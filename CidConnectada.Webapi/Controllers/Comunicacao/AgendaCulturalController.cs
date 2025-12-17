using CidConnectada.Entities.Model.Comunicacao;
using CidConnectada.Services.Impl.AWS;
using CidConnectada.Services.Intf.AWS;
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
    [RoutePrefix("api/AgendaCultural")]
    [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO,CIDADAO")]
    public class AgendaCulturalController : BaseWebApiController<AgendaCultural, AgendaCulturalDto, IAgendaCulturalService, string, int, string>
    {
        private readonly IAWSS3Service AWSS3Service;
        public AgendaCulturalController(
            IAgendaCulturalService cadService,
            AutoMapper.IMapper mapper, Func<ContextRequest<int, string>> contextFactory,
            IAWSS3Service aWSS3Service
        )
            : base(cadService, mapper, contextFactory)
        {
            AWSS3Service = aWSS3Service;
            GeneroEntidade = GenreEnum.Female;
            Title = "AgendaCultural";
        }

        #region CRUD

        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(AgendaCulturalDto))]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public override async Task<IHttpActionResult> Post(AgendaCulturalDto model)
        {
            return await base.Post(model);
        }

        [HttpGet]
        [Route("GetOne")]
        [ResponseType(typeof(AgendaCulturalDto))]
        public override async Task<IHttpActionResult> GetOne(string id)
        {
            return await base.GetOne(id);
        }

        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(AgendaCulturalDto))]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public override async Task<IHttpActionResult> Put(AgendaCulturalDto model)
        {
            return await base.Put(model);
        }

        [HttpGet]
        [Route("GetFiltered")]
        //[ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        [ResponseType(typeof(SearchResultDto<AgendaCulturalDto>))]
        public override async Task<IHttpActionResult> GetFiltered([FromUri] ContainsFilter filter)
        {
            return await base.GetFiltered(filter);
        }

        #endregion

        #region Custom

        protected async override Task IncluirAsync(AgendaCultural entity)
        {
            Delegate upload = new Func<object, Task>(async (ent) => await AWSS3Service.UploadAsync((AgendaCultural)ent));
            await cadService.IncluirAsync(entity, upload);
        }

        protected async override Task AlterarAsync(AgendaCultural entity)
        {
            Delegate upload = new Func<object, Task>(async (ent) => await AWSS3Service.UploadAsync((AgendaCultural)ent));
            await cadService.AlterarAsync(entity, upload);
        }

        [HttpDelete]
        [Route("Delete")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public override async Task<IHttpActionResult> Delete(string id)
        {
            AgendaCultural entity = await cadService.ObterAsync(id);
            Delegate deleteS3 = new Func<object, Task>(async (ent) => await AWSS3Service.DeleteAsync((AgendaCultural)ent));
            await cadService.DeleteAsync(entity, deleteS3);

            return Ok();
        }

        #endregion

    }
}