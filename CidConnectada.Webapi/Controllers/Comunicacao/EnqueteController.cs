using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Comunicacao;
using CidConnectada.Entities.Model.Dto;
using CidConnectada.Services.Intf.Comunicacao;
using CidConnectada.Services.Intf.Organograma;
using CidConnectada.Webapi.Models.Comunicacao;
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

namespace CidConnectada.Webapi.Controllers.Comunicacao
{
    [RoutePrefix("api/Enquete")]
    [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO,CIDADAO")]
    public class EnqueteController : MasterDetailWebApiController<Enquete, EnqueteDto, IEnqueteService, int, EnqueteOpcao, EnqueteOpcaoDto, EnqueteOpcaoKey,
        int, string>
    {

        public EnqueteController(IEnqueteService cadService, AutoMapper.IMapper mapper, Func<ContextRequest<int, string>> contextFactory)
            : base(cadService, mapper, contextFactory)
        {
            GeneroEntidade = GenreEnum.Female;
            Title = "Enquete";
        }

        #region Custom

        [HttpPost]
        [Route("Responder")]
        [ResponseType(typeof(EnqueteDto))]
        public async Task<IHttpActionResult> Responder(EnqueteRespostaDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Enquete enquete = await cadService.ObterAsync(model.enqueteId);

            if (enquete.VigenciaInicio > DateTime.Now || enquete.VigenciaFinal < DateTime.Now)
                return BadRequest("Não é possível responder uma enquete fora de vigência");

            if (await cadService.EstaRespondida(enquete.Key, ((Usuario)Context.User).Key))
                return BadRequest("Enquete já respondida. Não é possível alterar a resposta.");

            if (!enquete.IsMultiVal && model.opcoes.Count > 1)
                return BadRequest("Não é possivel marcar mais de uma opção nesta enquete.");

            IList<EnqueteResposta> respostas = AMapper.Map<IList<EnqueteResposta>>(model);
            cadService.IncluirEnqueteResposta(respostas);

            return await base.GetOne(model.enqueteId);
        }

        [HttpGet]
        [Route("GetVigentes")]
        [ResponseType(typeof(SearchResultDto<EnqueteDto>))]
        public async Task<IHttpActionResult> GetVigentes([FromUri] ContainsFilter filter)
        {
            Context.CacheRequest.Add("GetVigentes", true);
            return await base.GetFiltered(filter);
        }

        [HttpGet]
        [Route("GetResultado")]
        [ResponseType(typeof(EnqueteResultadoDto))]
        public async Task<IHttpActionResult> GetResultado(int id)
        {
            return Ok(await cadService.GetResultado(id));
        }

        #endregion

        #region CRUD

        [HttpPost]
        [Route("Post")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        [ResponseType(typeof(EnqueteDto))]
        public override async Task<IHttpActionResult> Post(EnqueteDto model)
        {
            return await base.Post(model);
        }

        [HttpGet]
        [Route("GetOne")]
        [ResponseType(typeof(EnqueteDto))]
        public override async Task<IHttpActionResult> GetOne(int id)
        {
            return await base.GetOne(id);
        }

        [HttpGet]
        [Route("GetAll")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        [ResponseType(typeof(IList<EnqueteDto>))]
        public override async Task<IHttpActionResult> GetAll()
        {
            return await base.GetAll();
        }

        [HttpPut]
        [Route("Put")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        [ResponseType(typeof(EnqueteDto))]
        public override async Task<IHttpActionResult> Put(EnqueteDto model)
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
        [ResponseType(typeof(SearchResultDto<EnqueteDto>))]
        public override async Task<IHttpActionResult> GetFiltered([FromUri] ContainsFilter filter)
        {
            return await base.GetFiltered(filter);
        }

        #endregion
    }
}