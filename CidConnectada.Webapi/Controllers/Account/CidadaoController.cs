using CidConnectada.Entities.Model.Account;
using CidConnectada.Services.Intf.Account;
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

namespace CidConnectada.Webapi.Controllers.Account
{
    [ClaimsAuthorize]
    [RoutePrefix("api/Cidadao")]
    public class CidadaoController : UsuarioGenericController<Cidadao, CidadaoDto, ICidadaoService>
    {
        public CidadaoController(ICidadaoService cadService, AutoMapper.IMapper mapper, Func<ContextRequest<int, string>> contextFactory)
            : base(cadService, mapper, contextFactory)
        {
            GeneroEntidade = GenreEnum.Male;
            Title = "Cidadão";
        }

        #region CRUD

        [HttpPost]
        [Route("Post")]
        public override async Task<IHttpActionResult> Post(CidadaoDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.rolesList.Clear();
            model.rolesList.Add(new piLookupModel<string>
            {
                value = "0",
                text = "CIDADAO"
            });

            return await base.Post(model);
        }

        [HttpGet]
        [Route("GetOne")]
        [ResponseType(typeof(CidadaoDto))]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO,CIDADAO")]
        public override async Task<IHttpActionResult> GetOne(int id)
        {
            if (Context.User is Cidadao cidadao && cidadao.Key != id)
                return BadRequest("Acessar as informações de outro cidadão não é permitido.");

            return await base.GetOne(id);
        }

        [HttpPut]
        [Route("Put")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO,CIDADAO")]
        public async Task<IHttpActionResult> Put(CidadaoEditDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            if (((Usuario)Context.User).Key != model.key)
                return BadRequest("Alterar as informações de outro cidadão não é permitido.");

            Usuario entity = await cadService.ObterAsync(model.key, new string[1] { "AspNetUsers" });

            if (!String.IsNullOrWhiteSpace(model.email) && entity.AspNetUsers.Email != model.email)
                Context.CacheRequest.Add("email", model.email);

            return await base.Put<CidadaoEditDto, Cidadao>(model);
        }

        protected override object GetPutResult<TDto>(Cidadao entity)
        {
            return AMapper.Map<CidadaoDto>(entity);
        }

        [HttpDelete]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        [Route("Delete")]
        public override async Task<IHttpActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }

        [HttpGet]
        [Route("GetPage")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        [ResponseType(typeof(SearchResultDto<CidadaoDto>))]
        public override async Task<IHttpActionResult> GetPage([FromUri] SearchOptions options)
        {
            return await base.GetPage(options);
        }

        [HttpGet]
        [Route("GetFiltered")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        [ResponseType(typeof(SearchResultDto<CidadaoDto>))]
        public override async Task<IHttpActionResult> GetFiltered([FromUri] ContainsFilter filter)
        {
            return await base.GetFiltered(filter);
        }

        #endregion

        #region Custom

        [HttpGet]
        [Route("MeuPerfil")]
        [ResponseType(typeof(CidadaoDto))]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO,CIDADAO")]
        public override async Task<IHttpActionResult> MeuPerfil()
        {
            return await base.MeuPerfil();
        }

        #endregion
    }
}