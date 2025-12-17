using CidConnectada.Entities.Model.Enums;
using CidConnectada.Entities.Model.Organograma;
using CidConnectada.Services.Intf.Account;
using CidConnectada.Webapi.Models.Organograma;
using System;
using System.Linq;
using Zenite.Pi.Context;

namespace CidConnectada.Webapi.Code.Map.Organograma
{
    public class OrganogramaEntityToDtoProfile : EntityToDtoBaseProfile
    {
        protected IUsuarioService UsuarioService => GetService<IUsuarioService>();
        public OrganogramaEntityToDtoProfile(
            Func<ContextRequest<int, string>> contextFactory
        )
            : base(contextFactory)
        {
            CreateMap<Prefeitura, PrefeituraDto>()
                .ForMember(dest => dest.enderecoId, opt =>
                {
                    opt.PreCondition(src => src.Endereco != null);
                    opt.MapFrom(src => src.Endereco.Key);
                })
                .ForMember(dest => dest.enderecoCompleto, opt =>
                {
                    opt.PreCondition(src => src.Endereco != null);
                    opt.MapFrom(src => src.Endereco.EnderecoCompleto);
                })
                .ForMember(dest => dest.admin, opt => opt.MapFrom(src => UsuarioService.GetPrincipal()))
                .ForMember(dest => dest.primaryMainColor, opt => opt.MapFrom(src => string.Concat(src.PrimaryMainColor.Prepend('#'))))
                .ForMember(dest => dest.primaryDarkColor, opt => opt.MapFrom(src => string.Concat(src.PrimaryDarkColor.Prepend('#'))))
                .ForMember(dest => dest.primaryLightColor, opt => opt.MapFrom(src => string.Concat(src.PrimaryLightColor.Prepend('#'))))
                .ForMember(dest => dest.secondaryMainColor, opt => opt.MapFrom(src => string.Concat(src.SecondaryMainColor.Prepend('#'))))
                .ForMember(dest => dest.secondaryDarkColor, opt => opt.MapFrom(src => string.Concat(src.SecondaryDarkColor.Prepend('#'))))
                .ForMember(dest => dest.secondaryLightColor, opt => opt.MapFrom(src => string.Concat(src.SecondaryLightColor.Prepend('#'))))
                .ForMember(dest => dest.base64LogoHeader, opt => opt.Ignore())
                .ForMember(dest => dest.base64LogoHorizontal, opt => opt.Ignore())
                .ForMember(dest => dest.base64LogoVertical, opt => opt.Ignore())
                .ForMember(dest => dest.extensaoLogoHeader, opt => opt.Ignore())
                .ForMember(dest => dest.extensaoLogoHorizontal, opt => opt.Ignore())
                .ForMember(dest => dest.extensaoLogoVertical, opt => opt.Ignore());

            CreateMap<Prefeitura, PrefeituraViewDto>()
                .ForMember(dest => dest.enderecoId, opt =>
                {
                    opt.PreCondition(src => src.Endereco != null);
                    opt.MapFrom(src => src.Endereco.Key);
                })
                .ForMember(dest => dest.enderecoCompleto, opt =>
                {
                    opt.PreCondition(src => src.Endereco != null);
                    opt.MapFrom(src => src.Endereco.EnderecoCompleto);
                });

            CreateMap<Prefeitura, RedesSociaisDto>();

            CreateMap<Secretaria, OrdemHomeDto<string>>();

            CreateMap<Secretaria, SecretariaDto>()
                .ForMember(dest => dest.secretariaMenuList, opt =>
                {
                    opt.PreCondition(ctx => IgnoreMethods(ctx, "SecretariaController", "GetHome"));
                    opt.MapFrom(src => src.SecretariaMenuSet);
                });

            CreateMap<SecretariaMenu, SecretariaMenuDto>()
                .ForMember(dest => dest.rotaInternaId, opt => opt.MapFrom(src => src.RotaInterna.Key))
                .ForMember(dest => dest.path, opt => opt.MapFrom((src, dest, member, ctx) =>
                {
                    if (src.RotaTipoEnum == RotaTipoEnum.Link_Interno)
                    {
                        return src.RotaInterna.Path;
                    }
                    return src.Path;
                }))
                .ForMember(dest => dest.rotaTipoEnumNome, opt => opt.MapFrom(src => src.RotaTipoEnum.ToString().Replace("_", " ")));
        }
    }
}