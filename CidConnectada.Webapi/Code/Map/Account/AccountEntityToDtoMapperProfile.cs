using CidConnectada.Entities.Model.Account;
using CidConnectada.Webapi.Models;
using CidConnectada.Webapi.Models.Account;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using Zenite.Pi.Context;
using Zenite.Pi.Web.Models.Pesquisa;

namespace CidConnectada.Webapi.Code.Map.Account
{
    public class AccountEntityToDtoMapperProfile : EntityToDtoBaseProfile
    {
        public AccountEntityToDtoMapperProfile(Func<ContextRequest<int, string>> contextFactory)
            : base(contextFactory)
        {
            CreateMap<Usuario, UsuarioDto>()
                .ForMember(dest => dest.telefone, opt => opt.MapFrom(src => src.AspNetUsers.PhoneNumber))
                .ForMember(dest => dest.email, opt => opt.MapFrom(e => e.AspNetUsers.Email))
                .ForMember(dest => dest.rolesList, opt => opt.MapFrom(src => src.AspNetUsers.AspNetUserRolesSet))
                .ForMember(dest => dest.tenantId, opt => opt.MapFrom(src => src.Prefeitura.Key));

            CreateMap<Funcionario, FuncionarioDto>()
                .IncludeBase<Usuario, UsuarioDto>();

            CreateMap<Cidadao, CidadaoDto>()
                .IncludeBase<Usuario, UsuarioDto>()
                .ForMember(dest => dest.bairroId, opt => opt.MapFrom(src => src.Bairro.Key));

            CreateMap<IdentityRole, RolesViewModel>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(e => e.Id))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(e => e.Name));

            CreateMap<AspNetUserRoles, piLookupModel<string>>()
                .IncludeMembers(src => src.AspNetRoles);

            CreateMap<Usuario, piLookupModel<long>>()
                .ForMember(dest => dest.value, opt => opt.MapFrom(src => src.Key))
                .ForMember(dest => dest.group,
                    opt => opt.MapFrom(src =>
                        src.AspNetUsers.AspNetUserRolesSet.Any()
                            ? src.AspNetUsers.AspNetUserRolesSet.First().AspNetRoles.Name
                            : ""))
                .ForMember(dest => dest.text,
                    opt => opt.MapFrom(src => $"{src.UserName}"));

            CreateMap<AspNetRoles, piLookupModel<string>>()
                .ForMember(dest => dest.value, opt => opt.MapFrom(src => src.Key))
                .ForMember(dest => dest.group,
                    opt => opt.MapFrom(src =>
                        src.AspNetUserRolesSet.Any() ? src.AspNetUserRolesSet.First().AspNetRoles.Name : ""))
                .ForMember(dest => dest.text,
                    opt => opt.MapFrom(src => String.Format("{0} ({1})", src.Name, src.Description)));
        }
    }
}