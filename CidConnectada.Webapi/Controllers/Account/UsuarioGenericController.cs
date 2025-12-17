using CidConnectada.Dao.Identity;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Identity;
using CidConnectada.Services.Impl.Identity;
using CidConnectada.Services.Intf.Account;
using CidConnectada.Webapi.Models.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Zenite.Pi.Context;
using Zenite.Pi.Entities.Enums;
using Zenite.Pi.Entities.Model.Search;
using Zenite.Pi.Exceptions;
using Zenite.Pi.Web.Models.Cadastro;
using Zenite.Pi.Web.WebApi;

namespace CidConnectada.Webapi.Controllers.Account
{
    public abstract class
        UsuarioGenericController<TEntity, TDto, TIService> : BaseWebApiController<TEntity, TDto, TIService, int, int,
        string>
        where TEntity : Usuario
        where TDto : UsuarioDto
        where TIService : IUsuarioGenericService<TEntity>
    {
        public UsuarioGenericController(TIService cadService, AutoMapper.IMapper mapper, Func<ContextRequest<int, string>> contextFactory)
            : base(cadService, mapper, contextFactory)
        {
            GeneroEntidade = GenreEnum.Male;
            Title = "User";
        }

        #region Services

        protected IUsuarioService UsuarioService
        {
            get => GetService<IUsuarioService>();
        }

        private ApplicationUserManager _userManager;

        protected ApplicationUserManager UserManager
        {
            get => _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        private RoleManager<IdentityRole> _roleManager;

        protected RoleManager<IdentityRole> RoleManager
        {
            get => _roleManager ?? new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            private set => _roleManager = value;
        }

        #endregion

        #region CRUD

        protected override object GetPostResult<TDto1>(TEntity entity)
        {
            return new
            {
                key = entity.Key
            };
        }

        public override async Task<IHttpActionResult> Post(TDto model)
        {
            if (!Context.CacheRequest.TryGetValue("TenantId", out object tenantId))
                return BadRequest("O Header TenantId não foi informado, ou não existe um usuário logado.");
            model.tenantId = Convert.ToInt32(tenantId);
            
            Context.CacheRequest.Add("UsuarioDto", model);
            
            return await base.Post(model);
        }

        protected override async Task IncluirAsync(TEntity entity)
        {
            if (!Context.CacheRequest.TryGetValue("UsuarioDto", out object value))
                throw new PiBusinessException("Erro ao tentar recuperar o UsuarioDto do Contexto da aplicação.");
            
            TDto model = (TDto)value;
            
            string username = model.email;
            IList<string> roleNamesList = model.rolesList.Select(r => r.text).ToList();
            
            if (roleNamesList.Contains("CIDADAO"))
                username = model.telefone;
            
            ApplicationUser appUser = AMapper.Map<ApplicationUser>(model, opt => opt.Items["username"] = username);
            
            //YURI: apagar usuario ja existente caso nao esteja com cadastro confirmado
            await cadService.DeleteIfPhoneNotConfirmedAsync(username);
            await cadService.IncluirAsync(entity, appUser, model.password);
        }

        public override async Task<IHttpActionResult> GetPage([FromUri] SearchOptions options)
        {
            return await base.GetPage(options);
        }

        public override async Task<IHttpActionResult> GetFiltered([FromUri] ContainsFilter filter)
        {
            return await base.GetFiltered(filter);
        }

        protected virtual async Task AddRolesAsync(string id, TDto model, TEntity entity)
        {
            foreach (var role in model.rolesList)
                if (!String.IsNullOrEmpty(role.text))
                    await UserManager.AddToRoleAsync(id, role.text);
        }

        #endregion

        #region Custom

        public virtual async Task<IHttpActionResult> MeuPerfil()
        {
            return await base.GetOne(((Usuario)Context.User).Key);
        }

        #endregion
    }
}