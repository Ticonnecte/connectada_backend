using CidConnectada.Entities.Model.Account;
using CidConnectada.Services.Intf.Account;
using CidConnectada.Services.Intf.Organograma;
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
    [RoutePrefix("api/Usuario")]
    [ClaimsAuthorize(Roles = "SA,ADMIN")]
    public class UsuarioController : UsuarioGenericController<Usuario, UsuarioDto, IUsuarioService>
    {
        public UsuarioController(IUsuarioService cadService, AutoMapper.IMapper mapper, Func<ContextRequest<int, string>> contextFactory)
            : base(cadService, mapper, contextFactory)
        {
            GeneroEntidade = GenreEnum.Male;
            Title = "Usuário";
        }

        #region CRUD

        [HttpPost]
        [Route("PostAdmin")]
        public async Task<IHttpActionResult> PostAdmin(UsuarioDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.rolesList.Clear();
            model.rolesList.Add(new piLookupModel<string>
            {
                value = "0",
                text = "ADMIN"
            });

            return await base.Post(model);
        }

        [HttpGet]
        [Route("GetOne")]
        [ResponseType(typeof(UsuarioDto))]
        public override async Task<IHttpActionResult> GetOne(int id)
        {
            return await base.GetOne(id);
        }

        [HttpPut]
        [Route("Put")]
        public async Task<IHttpActionResult> Put(UsuarioEditDto model)
        {
            Usuario entity = await cadService.ObterAsync(model.key, new string[1] { "AspNetUsers" });

            if (!(entity is Cidadao) 
                && !String.IsNullOrWhiteSpace(model.telefone) 
                && entity.AspNetUsers.PhoneNumber != model.telefone)
                Context.CacheRequest.Add("telefone", model.telefone);
            
            if (entity is Cidadao 
                && !String.IsNullOrWhiteSpace(model.email) 
                && entity.AspNetUsers.Email != model.email)
                Context.CacheRequest.Add("email", model.email);
            
            return await base.Put<UsuarioEditDto, Usuario>(model);
        }
        
        protected override object GetPutResult<TDto>(Usuario entity)
        {
            return AMapper.Map<UsuarioDto>(entity);
        }

        [HttpDelete]
        [Route("Delete")]
        public override async Task<IHttpActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }

        #endregion
        
        #region Custom 
        
        [HttpGet]
        [Route("MeuPerfil")]
        [ResponseType(typeof(UsuarioDto))]
        public override async Task<IHttpActionResult> MeuPerfil()
        {
            return await base.MeuPerfil();
        }
        
        [HttpGet]
        [ClaimsAuthorize(Roles = "SA,ADMIN")]
        [Route("GetFilteredAdmin")]
        [ResponseType(typeof(SearchResultDto<UsuarioDto>))]
        public async Task<IHttpActionResult> GetFilteredAdmin([FromUri] ContainsFilter filter)
        {
            Context.CacheRequest.Add("GetFilteredAdmin", true);
            return await base.GetFiltered(filter);
        }

        [HttpPut]
        [ClaimsAuthorize(Roles = "SA,ADMIN")]
        [Route("UpdateAdmin")]
        public async Task<IHttpActionResult> UpdateAdmin(UsuarioDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (((Usuario)Context.User).Key != model.key && !Context.IsAdmin)
                return BadRequest("Alterar as informações de um administrador não é permitido.");

            Usuario entity = await cadService.ObterAsync(model.key, new string[1] { "AspNetUsers" });

            if (!String.IsNullOrWhiteSpace(model.telefone) && entity.AspNetUsers.PhoneNumber != model.telefone)
                Context.CacheRequest.Add("telefone", model.telefone);

            return await base.Put<UsuarioDto, Usuario>(model);
        }
        #endregion

    }
}
