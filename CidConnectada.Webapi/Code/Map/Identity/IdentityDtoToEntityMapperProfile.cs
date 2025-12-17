using CidConnectada.Entities.Model.Identity;
using CidConnectada.Services.Intf.Account;
using CidConnectada.Webapi.Models.Account;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using Zenite.Pi.Context;
using Zenite.Pi.Web.Models.Pesquisa;

namespace CidConnectada.Webapi.Code.Map.Identity
{
    // Dto => Entity
    public class IdentityDtoToEntityMapperProfile : DtoToEntityBaseProfile
    {
        #region Services

        protected IUsuarioService UsuarioService => GetService<IUsuarioService>();


        #endregion
        public IdentityDtoToEntityMapperProfile(Func<ContextRequest<int, string>> contextFactory
        )
            : base(contextFactory)
        {

            CreateMap<UsuarioDto, ApplicationUser>()
                .ForMember(dest => dest.Id, opt =>
                {
                    opt.PreCondition(src => src.isNew);
                    opt.MapFrom(src => Guid.NewGuid().ToString());
                })
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.telefone))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom((src, dest, member, ctx) => ctx.Items["username"]))
                .ForMember(dest => dest.TenantKey, opt =>
                {
                    opt.PreCondition(ctx => !ctx.Items.TryGetValue("Caller", out object caller)
                        || (string)caller != "PrefeituraController.Post");
                    opt.PreCondition(src => src.isNew);
                    opt.MapFrom(src => src.tenantId);
                })
                .ForMember(dest => dest.Roles, opt => opt.MapFrom((src) => src.rolesList));

            CreateMap<piLookupModel<string>, IdentityUserRole>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => UsuarioService.GetRoleIdByName(src.text)));

        }

    }
}