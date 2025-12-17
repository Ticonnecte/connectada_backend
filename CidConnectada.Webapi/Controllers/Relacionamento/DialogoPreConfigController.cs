using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Relacionamento;
using CidConnectada.Services.Intf.Organograma;
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
    [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO,CIDADAO")]
    [RoutePrefix("api/DialogoPreConfig")]
    public class DialogoPreConfigController : BaseWebApiController<DialogoPreConfig, DialogoPreConfigDto, IDialogoPreConfigService, int, int, string>
    {
        public DialogoPreConfigController(IDialogoPreConfigService cadService, AutoMapper.IMapper mapper, Func<ContextRequest<int, string>> contextFactory)
            : base(cadService, mapper, contextFactory)
        {
            GeneroEntidade = GenreEnum.Female;
            Title = "Pré-Configuração de Diálogo";
        }

        #region CRUD

        [HttpPost]
        [Route("Post")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        [ResponseType(typeof(DialogoPreConfigDto))]
        public override async Task<IHttpActionResult> Post(DialogoPreConfigDto model)
        {
            return await base.Post(model);
        }
        
        [HttpPut]
        [Route("Put")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        [ResponseType(typeof(DialogoPreConfigDto))]
        public override async Task<IHttpActionResult> Put(DialogoPreConfigDto model)
        {
            return await base.Put(model);
        }
        
        [HttpGet]
        [Route("GetOne")]
        [ResponseType(typeof(DialogoPreConfigDto))]
        public override async Task<IHttpActionResult> GetOne(int id)
        {
            return await base.GetOne(id);
        }
        
        [HttpDelete]
        [Route("Delete")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public override async Task<IHttpActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }

        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IList<DialogoPreConfigDto>))]
        public override async Task<IHttpActionResult> GetAll()
        {
            return await base.GetAll();
        }

        [HttpGet]
        [Route("GetFiltered")]
        [ResponseType(typeof(SearchResultDto<DialogoPreConfigDto>))]
        public override async Task<IHttpActionResult> GetFiltered([FromUri] ContainsFilter filter)
        {
            return Ok(await base.GetFilteredGeneric<DialogoPreConfigDto>(filter));
        }

        #endregion

    }
}