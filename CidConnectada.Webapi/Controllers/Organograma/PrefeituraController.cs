using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Identity;
using CidConnectada.Entities.Model.Organograma;
using CidConnectada.Services.Intf.Account;
using CidConnectada.Services.Intf.AWS;
using CidConnectada.Services.Intf.Messaging;
using CidConnectada.Services.Intf.Notificacao;
using CidConnectada.Services.Intf.Organograma;
using CidConnectada.Webapi.Models.Account;
using CidConnectada.Webapi.Models.Organograma;
using CidConnectada.Website.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Zenite.Pi.Context;
using Zenite.Pi.Entities.Enums;
using Zenite.Pi.Entities.Model.Search;
using Zenite.Pi.Exceptions;
using Zenite.Pi.Web.Models.Cadastro;
using Zenite.Pi.Web.Models.Pesquisa;
using Zenite.Pi.Web.WebApi;
using Zenite.Wa.Models.Zapi.Instance;

namespace CidConnectada.Webapi.Controllers.Account
{
    [ClaimsAuthorize]
    [RoutePrefix("api/Prefeitura")]
    public class PrefeituraController : BaseWebApiController<Prefeitura, PrefeituraDto, IPrefeituraService, int, int, string>
    {
        public PrefeituraController(
            IPrefeituraService cadService,
            AutoMapper.IMapper mapper,
            Func<ContextRequest<int, string>> contextFactory,
            IZApiService zApiService,
            IUsuarioService usuarioService,
            IAWSS3Service s3Service
        )
            : base(cadService, mapper, contextFactory)
        {
            ZApiService = zApiService;
            UsuarioService = usuarioService;
            S3Service = s3Service;
            GeneroEntidade = GenreEnum.Female;
            Title = "Prefeitura";
        }

        #region Services

        private readonly IZApiService ZApiService;

        private readonly IUsuarioService UsuarioService;

        private readonly IAWSS3Service S3Service;

        #endregion

        #region CRUD

        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(PrefeituraDto))]
        [ClaimsAuthorize(Roles = "SA")]
        public async Task<IHttpActionResult> Post(PrefeituraDto model)
        {
            Context.CacheRequest.Add("AdminDto", model.admin);
            return await base.Post<PrefeituraDto, Prefeitura>(model);
        }

        protected override async Task IncluirAsync(Prefeitura entity)
        {
            if (!Context.CacheRequest.TryGetValue("AdminDto", out object value))
                throw new PiBusinessException("Erro ao tentar recuperar o AdminDto do contexto da aplicação.");

            var adminDto = value as UsuarioDto;
            adminDto.isNew = true;
            string username = adminDto.email;
            
            adminDto.rolesList.Clear();
            adminDto.rolesList.Add(new piLookupModel<string>()
            {
                text = "ADMIN"
            });
            
            ApplicationUser appUser = AMapper.Map<ApplicationUser>(adminDto, opt =>
            {
                opt.Items["username"] = username;
                opt.Items["Caller"] = $"{nameof(PrefeituraController)}.{nameof(Post)}";
            });
            
            Usuario user = AMapper.Map<Funcionario>(adminDto, opt => 
                opt.Items["Caller"] = $"{nameof(PrefeituraController)}.{nameof(Post)}");

            Delegate upload = new Func<object, HttpContext, Task>(async (ent, httpContext) => await cadService.UploadLogos((Prefeitura)ent));
            await cadService.IncluirPlusAsync(entity, user, appUser, upload);
        }

        [HttpGet]
        [Route("GetOne")]
        [ResponseType(typeof(PrefeituraDto))]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public override async Task<IHttpActionResult> GetOne(int id)
        {
            Context.CacheRequest.Remove("TenantId");
            Context.CacheRequest.Add("TenantId", id);
            return await base.GetOne(id);
        }

        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(PrefeituraDto))]
        [ClaimsAuthorize(Roles = "SA")]
        public override async Task<IHttpActionResult> Put(PrefeituraDto model)
        {
            return await base.Put(model);
        }

        [HttpDelete]
        [Route("Delete")]
        [ClaimsAuthorize(Roles = "SA")]
        public override async Task<IHttpActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }

        [HttpGet]
        [Route("GetPage")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        [ResponseType(typeof(SearchResultDto<PrefeituraViewDto>))]
        public override async Task<IHttpActionResult> GetPage([FromUri] SearchOptions options)
        {
            return Ok(await base.GetPageGeneric<PrefeituraViewDto>(options));
        }

        [HttpGet]
        [Route("GetFiltered")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        [ResponseType(typeof(SearchResultDto<PrefeituraViewDto>))]
        public override async Task<IHttpActionResult> GetFiltered([FromUri] ContainsFilter filter)
        {
            return Ok(await base.GetFilteredGeneric<PrefeituraViewDto>(filter));
        }

        #endregion

        #region Custom

        [HttpGet]
        [Route("GetUsersWhastappEnabled")]
        [ResponseType(typeof(piLookupModel<int>))]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public async Task<IHttpActionResult> GetUsersWhastappEnabled()
        {
            IList<Usuario> userList = await UsuarioService.GetWhatsAppEnabledAsync();
            return Ok(userList.Select(u => new piLookupModel<int> { value = u.Key, text = u.NomeCompleto, group = u.GetType().Name.Split('_')[0] }));
        }
        
        [HttpPut]
        [Route("PutRedesSociais")]
        [ResponseType(typeof(RedesSociaisDto))]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public async Task<IHttpActionResult> PutRedesSociais(RedesSociaisDto model)
        {
            int id = 0;
            if (Context.CacheRequest.TryGetValue("TenantId", out object tenantId))
                id = (int)tenantId;

            Prefeitura entity = await cadService.ObterAsync(id, null);
            entity = AMapper.Map(model, entity);
            await cadService.UpdateRedesSociaisAsync(entity);
            return Ok(AMapper.Map<RedesSociaisDto>(entity));
        }
        
        [HttpGet]
        [Route("GetRedesSociais")]
        [ResponseType(typeof(RedesSociaisDto))]
        public async Task<IHttpActionResult> GetRedesSociais()
        {
            int id = 0;
            if (Context.CacheRequest.TryGetValue("TenantId", out object tenantId))
                id = (int)tenantId;

            Prefeitura entity = await cadService.ObterAsync(id, null);
            return Ok(AMapper.Map<RedesSociaisDto>(entity));
        }

        [HttpGet]
        [Route("GetQrCodeZApi")]
        [ResponseType(typeof(ZApiQrCode64ResultDto))]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public async Task<IHttpActionResult> GetQrCodeZApi()
        {
            return Ok(await ZApiService.GetQrCodeBase64Async());
        }

        [HttpGet]
        [Route("DisconnectZApi")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public async Task<IHttpActionResult> DisconnectZApi()
        {
            return Ok(await ZApiService.DisconnectAsync());
        }

        [HttpGet]
        [Route("IsConnectedZApi")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public async Task<IHttpActionResult> IsConnectedZApi()
        {
            return Ok(await ZApiService.ConnectedAsync());
        }

        [HttpGet]
        [Route("GetStatusZApi")]
        [ResponseType(typeof(ZApiStatusInstanceDto))]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public async Task<IHttpActionResult> GetStatusZApi()
        {
            return Ok(await ZApiService.GetStatusAsync());
        }

        [HttpGet]
        [Route("GetS3Regions")]
        [ResponseType(typeof(IList<piLookupModel<string>>))]
        [ClaimsAuthorize(Roles = "SA")]
        public async Task<IHttpActionResult> GetS3Regions()
        {
            return Ok(S3Service.GetRegions().Select(r => new piLookupModel<string>() { value = r.Key, text = r.Value, group = "S3 Region"}));
        }

        #endregion

    }
}
