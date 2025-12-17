using CidConnectada.Entities.Model.Account;
using CidConnectada.Services.Intf.Account;
using CidConnectada.Services.Intf.Messaging;
using CidConnectada.Webapi.Models.Account;
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
using Zenite.Wa.Models.Zapi.Instance;

namespace CidConnectada.Webapi.Controllers.Account
{
    [RoutePrefix("api/Funcionario")]
    [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
    public class FuncionarioController : UsuarioGenericController<Funcionario, FuncionarioDto, IFuncionarioService>
    {
        public FuncionarioController(
            IFuncionarioService cadService,
            AutoMapper.IMapper mapper,
            Func<ContextRequest<int, string>> contextFactory,
            IZApiService zApiService
        )
            : base(cadService, mapper, contextFactory)
        {
            ZApiService = zApiService;
            GeneroEntidade = GenreEnum.Male;
            Title = "Funcionário";
        }

        #region Services

        private readonly IZApiService ZApiService;

        #endregion

        #region CRUD

        [HttpPost]
        [Route("Post")]
        [ClaimsAuthorize(Roles = "SA,ADMIN")]
        public override async Task<IHttpActionResult> Post(FuncionarioDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.rolesList.Clear();
            model.rolesList.Add(new piLookupModel<string>
            {
                value = "0",
                text = "FUNCIONARIO"
            });

            return await base.Post(model);
        }

        [HttpGet]
        [Route("GetOne")]
        [ResponseType(typeof(FuncionarioDto))]
        public override async Task<IHttpActionResult> GetOne(int id)
        {
            return await base.GetOne(id);
        }

        [HttpDelete]
        [Route("Delete")]
        public override Task<IHttpActionResult> Delete(int id)
        {
            return base.Delete(id);
        }

        [HttpPut]
        [Route("Put")]
        public async Task<IHttpActionResult> Put(FuncionarioEditDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            if (((Usuario)Context.User).Key != model.key && !Context.IsAdmin)
                return BadRequest("Alterar as informações de outro funcionário não é permitido.");
            
            Usuario entity = await cadService.ObterAsync(model.key, new string[1] { "AspNetUsers" });

            if (!String.IsNullOrWhiteSpace(model.telefone) && entity.AspNetUsers.PhoneNumber != model.telefone)
                Context.CacheRequest.Add("telefone", model.telefone);

            return await base.Put<FuncionarioEditDto, Funcionario>(model);
        }

        protected override object GetPutResult<TDto>(Funcionario entity)
        {
            return AMapper.Map<FuncionarioDto>(entity);
        }

        [HttpGet]
        [Route("GetPage")]
        [ResponseType(typeof(SearchResultDto<FuncionarioDto>))]
        public override async Task<IHttpActionResult> GetPage([FromUri] SearchOptions options)
        {
            return await base.GetPage(options);
        }

        [HttpGet]
        [Route("GetFiltered")]
        [ResponseType(typeof(SearchResultDto<FuncionarioDto>))]
        public override async Task<IHttpActionResult> GetFiltered([FromUri] ContainsFilter filter)
        {
            return await base.GetFiltered(filter);
        }

        #endregion

        #region Custom

        [HttpGet]
        [Route("MeuPerfil")]
        [ResponseType(typeof(FuncionarioDto))]
        public override async Task<IHttpActionResult> MeuPerfil()
        {
            return await base.MeuPerfil();
        }

        [HttpGet]
        [Route("GetQrCodeZApi")]
        [ResponseType(typeof(ZApiQrCode64ResultDto))]
        public async Task<IHttpActionResult> GetQrCodeZApi()
        {
            return Ok(await ZApiService.GetQrCodeBase64Async());
        }

        [HttpGet]
        [Route("DisconnectZApi")]
        public async Task<IHttpActionResult> DisconnectZApi()
        {
            return Ok(await ZApiService.DisconnectAsync());
        }

        [HttpGet]
        [Route("IsConnectedZApi")]
        public async Task<IHttpActionResult> IsConnectedZApi()
        {
            return Ok(await ZApiService.ConnectedAsync());
        }

        [HttpGet]
        [Route("GetStatusZApi")]
        [ResponseType(typeof(ZApiStatusInstanceDto))]
        public async Task<IHttpActionResult> GetStatusZApi()
        {
            return Ok(await ZApiService.GetStatusAsync());
        }

        #endregion
    }
}