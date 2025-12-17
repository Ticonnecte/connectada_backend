using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Relacionamento;
using CidConnectada.Services.Impl.AWS;
using CidConnectada.Services.Intf.AWS;
using CidConnectada.Services.Intf.Relacionamento;
using CidConnectada.Webapi.Models.Relacionamento;
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

namespace CidConnectada.Webapi.Controllers.Relacionamento
{
    [RoutePrefix("api/Dialogo")]
    [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO,CIDADAO")]
    public class DialogoController : BaseWebApiController<Dialogo, DialogoDto, IDialogoService, string, int, string>
    {
        private readonly IAWSS3Service AWSS3Service;

        public DialogoController(
            IDialogoService cadService,
            AutoMapper.IMapper mapper, Func<ContextRequest<int, string>> contextFactory,
            IAWSS3Service aWSS3Service
        )
            : base(cadService, mapper, contextFactory)
        {
            AWSS3Service = aWSS3Service;
            GeneroEntidade = GenreEnum.Male;
            Title = "Diálogo";
        }

        #region CRUD

        protected override object GetPostResult<TDto>(Dialogo entity)
        {
            return AMapper.Map<DialogoViewDto>(entity);
        }

        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(DialogoViewDto))]
        public override async Task<IHttpActionResult> Post(DialogoDto model)
        {
            return await base.Post(model);
        }

        [HttpGet]
        [Route("GetOne")]
        [ResponseType(typeof(DialogoViewDto))]
        public override async Task<IHttpActionResult> GetOne(string id)
        {
            Dialogo entity = await cadService.ObterAsync(id);

            if (entity.Cidadao != null && Context.User is Cidadao cidadao && cidadao.Key != entity.Cidadao.Key)
                return BadRequest("Acessar um diálogo de outro cidadão não é permitido.");

            return await base.GetOne<DialogoViewDto>(id);
        }

        [HttpGet]
        [Route("GetPage")]
        [ResponseType(typeof(SearchResultDto<DialogoSimpleDto>))]
        public override async Task<IHttpActionResult> GetPage([FromUri] SearchOptions options)
        {
            return Ok(await base.GetPageGeneric<DialogoSimpleDto>(options));
        }

        [HttpGet]
        [Route("GetFiltered")]
        [ResponseType(typeof(SearchResultDto<DialogoSimpleDto>))]
        public override async Task<IHttpActionResult> GetFiltered([FromUri] ContainsFilter filter)
        {
            return Ok(await base.GetFilteredGeneric<DialogoSimpleDto>(filter));
        }

        [HttpGet]
        [Route("GetFilteredPlus")]
        [ResponseType(typeof(SearchResultDto<DialogoViewDto>))]
        public async Task<IHttpActionResult> GetFilteredPlus([FromUri] ContainsFilter filter)
        {
            return Ok(await base.GetFilteredGeneric<DialogoViewDto>(filter));
        }

        #endregion

        #region Custom

        protected async override Task IncluirAsync(Dialogo entity)
        {
            Delegate upload = new Func<object, Task>(async (ent) => await AWSS3Service.UploadAsync((Dialogo)ent));
            await cadService.IncluirAsync(entity, upload);
        }

        protected async override Task AlterarAsync(Dialogo entity)
        {
            Delegate upload = new Func<object, Task>(async (ent) => await AWSS3Service.UploadAsync((Dialogo)ent));
            await cadService.AlterarAsync(entity, upload);
        }

        [HttpDelete]
        [Route("Delete")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public override async Task<IHttpActionResult> Delete(string id)
        {
            Dialogo entity = await cadService.ObterAsync(id);
            Delegate deleteS3 = new Func<object, Task>(async (ent) => await AWSS3Service.DeleteAsync((Dialogo)ent));
            await cadService.DeleteAsync(entity, deleteS3);

            return Ok();
        }

        [HttpGet]
        [Route("MeusDialogos")]
        [ResponseType(typeof(IList<DialogoViewDto>))]
        public async Task<IHttpActionResult> MeusDialogos()
        {
            return Ok(AMapper.Map<IList<DialogoSimpleDto>>(await cadService.GetMyDialogos()));
        }

        [HttpGet]
        [Route("GetHistorico")]
        [ResponseType(typeof(IList<HistoricoDialogoDto>))]
        public async Task<IHttpActionResult> GetHistorico(string id)
        {
            return Ok(AMapper.Map<DialogoHistoricoDto>(await cadService.ObterAsync(id)));
        }

        [HttpPut]
        [Route("SetDataPrevistaExecucao")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        [ResponseType(typeof(IList<HistoricoDialogoDto>))]
        public async Task<IHttpActionResult> SetDataPrevistaExecucao(ChangeDateDto model)
        {
            if (model.data < DateTime.Now)
                return BadRequest("Não é possivel alterar a data para o passado.");

            Dialogo entity = await cadService.ObterAsync(model.key);

            if (entity.DataPrevistaFinalizacao != null && entity.DataPrevistaFinalizacao > model.data)
                return BadRequest("A data prevista de execução não pode ficar após a data prevista de finalização.");

            entity.DataPrevistaExecuacao = model.data;
            await cadService.AlterarAsync(entity);
            return Ok($"{cadService.GetValorCampoDescritivoPadrao(entity)} está com a data prevista de execução em {model.data}.");
        }

        [HttpPut]
        [Route("SetDataPrevistaFinalizacao")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        [ResponseType(typeof(IList<HistoricoDialogoDto>))]
        public async Task<IHttpActionResult> SetDataPrevistaFinalizacao(ChangeDateDto model)
        {
            if (model.data < DateTime.Now)
                return BadRequest("Não é possivel alterar a data para o passado.");

            Dialogo entity = await cadService.ObterAsync(model.key);
            if (entity.DataPrevistaExecuacao == null)
                return BadRequest($"{cadService.GetValorCampoDescritivoPadrao(entity)} ainda não tem data prevista de execução");

            if (entity.DataPrevistaExecuacao > model.data)
                return BadRequest("A data prevista de execução não pode ficar após a data prevista de finalização.");

            entity.DataPrevistaFinalizacao = model.data;
            await cadService.AlterarAsync(entity);
            return Ok($"{cadService.GetValorCampoDescritivoPadrao(entity)} está com a data prevista de finalização em {model.data}.");
        }

        #endregion
        
    }
}