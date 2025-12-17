using CidConnectada.Entities.Model.Banners;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Entities.Model.Organograma;
using CidConnectada.Services.Intf.Banners;
using CidConnectada.Services.Intf.Local;
using CidConnectada.Services.Intf.Organograma;
using CidConnectada.Webapi.Models.Organograma;
using System;
using System.Linq;
using Zenite.Pi.Context;
using Zenite.Pi.IoC;

namespace CidConnectada.Webapi.Code.Map.Organograma
{
    public class OrganogramaDtoToEntityProfile : DtoToEntityBaseProfile
    {

        #region Services
        protected IPrefeituraService PrefeituraService => GetService<IPrefeituraService>();
        protected IEnderecoService EnderecoService => GetService<IEnderecoService>();
        protected IBannerService BannerService => GetService<IBannerService>();

        #endregion

        public OrganogramaDtoToEntityProfile(Func<ContextRequest<int, string>> contextFactory
        )
            : base(contextFactory)
        {
            CreateMap<PrefeituraDto, Prefeitura>()
                .BeforeMap((src, dest) =>
                {
                    if (src.isNew)
                    {
                        dest.Dominio = src.dominio;
                        dest.BucketName = $"{src.dominio}-{ApplicationContext.AppSettings["Amazon:S3:BucketSufixo"]}";
                    }
                })
                .ForMember(dest => dest.S3Region, opt => opt.SetMappingOrder(0))
                .ForMember(dest => dest.Endereco, opt =>
                {
                    opt.PreCondition(src => src.enderecoId != 0);
                    opt.MapFrom(src => EnderecoService.Obter(src.enderecoId, null));
                })
                .ForMember(dest => dest.LogoHeaderUrl, opt =>
                {
                    opt.SetMappingOrder(1);
                    opt.PreCondition((src, dest, ctx) => !String.IsNullOrEmpty(dest.BucketName) &&
                        !String.IsNullOrEmpty(src.base64LogoHeader) && !String.IsNullOrEmpty(src.extensaoLogoHeader));
                    opt.MapFrom((src, dest, member, ctx) => $"{dest.S3BaseUrl}{dest.GetS3Key(LogoPrefeituraEnum.Header, src.extensaoLogoHeader)}");
                })
                .ForMember(dest => dest.LogoHorizontalUrl, opt =>
                {
                    opt.SetMappingOrder(2);
                    opt.PreCondition((src, dest, ctx) => !String.IsNullOrEmpty(dest.BucketName) &&
                        !String.IsNullOrEmpty(src.base64LogoHorizontal) && !String.IsNullOrEmpty(src.extensaoLogoHorizontal));
                    opt.MapFrom((src, dest, member, ctx) => $"{dest.S3BaseUrl}{dest.GetS3Key(LogoPrefeituraEnum.Horizontal, src.extensaoLogoHorizontal)}");
                })
                .ForMember(dest => dest.LogoVerticalUrl, opt =>
                {
                    opt.SetMappingOrder(3);
                    opt.PreCondition((src, dest, ctx) => !String.IsNullOrEmpty(dest.BucketName) &&
                        !String.IsNullOrEmpty(src.base64LogoVertical) && !String.IsNullOrEmpty(src.extensaoLogoVertical));
                    opt.MapFrom((src, dest, member, ctx) => $"{dest.S3BaseUrl}{dest.GetS3Key(LogoPrefeituraEnum.Vertical, src.extensaoLogoVertical)}");
                })
                .ForMember(dest => dest.PrimaryMainColor, opt => opt.MapFrom(src => String.Concat(src.primaryMainColor.Where(Char.IsLetterOrDigit))))
                .ForMember(dest => dest.PrimaryDarkColor, opt => opt.MapFrom(src => String.Concat(src.primaryDarkColor.Where(Char.IsLetterOrDigit))))
                .ForMember(dest => dest.PrimaryLightColor, opt => opt.MapFrom(src => String.Concat(src.primaryLightColor.Where(Char.IsLetterOrDigit))))
                .ForMember(dest => dest.SecondaryMainColor, opt => opt.MapFrom(src => String.Concat(src.secondaryMainColor.Where(Char.IsLetterOrDigit))))
                .ForMember(dest => dest.SecondaryDarkColor, opt => opt.MapFrom(src => String.Concat(src.secondaryDarkColor.Where(Char.IsLetterOrDigit))))
                .ForMember(dest => dest.SecondaryLightColor, opt => opt.MapFrom(src => String.Concat(src.secondaryLightColor.Where(Char.IsLetterOrDigit))));

            CreateMap<RedesSociaisDto, Prefeitura>()
                .ForMember(dest => dest.Facebook, opt => opt.MapFrom(src => src.facebook))
                .ForMember(dest => dest.Youtube, opt => opt.MapFrom(src => src.youtube))
                .ForMember(dest => dest.Instagram, opt => opt.MapFrom(src => src.instagram))
                .ForMember(dest => dest.Site, opt => opt.MapFrom(src => src.site))
                .ForAllOtherMembers(opt => opt.UseDestinationValue());

            CreateMap<SecretariaDto, Secretaria>()
                .BeforeMap((src, dest) =>
                {
                    if (src.isNew)
                        src.key = Guid.NewGuid().ToString();
                })
                .ForMember(dest => dest.OrdemHome, opt => opt.Ignore());

            CreateMap<SecretariaMenuDto, SecretariaMenu>()
                .ForMember(dest => dest.RotaInterna, opt =>
                {
                    opt.PreCondition(src => src.rotaTipoEnum == RotaTipoEnum.Link_Interno);
                    opt.MapFrom((src, dest, member, ctx) =>
                    {
                        RotaInterna rotaInterna = BannerService.FindRotaById(src.rotaInternaId);
                        return rotaInterna;
                    });
                })
                .ForMember(dest => dest.Path, opt => opt.PreCondition(src => src.rotaTipoEnum == RotaTipoEnum.Link_Externo));
        }
    }
}