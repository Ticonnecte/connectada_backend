using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Banners;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Services.Intf.Account;
using CidConnectada.Services.Intf.Banners;
using CidConnectada.Webapi.Models.Banners;
using CidConnectada.Webapi.Models.Common;
using System;
using Zenite.Pi.Context;

namespace CidConnectada.Webapi.Code.Map.Banners
{
    // (Dto => Entity)
    public class BannerDtoToEntityMapperProfile : DtoToEntityBaseProfile
    {
        #region Services

        protected IBannerService BannerService => GetService<IBannerService>();
        protected IUsuarioService UsuarioService => GetService<IUsuarioService>();

        #endregion

        public BannerDtoToEntityMapperProfile(Func<ContextRequest<int, string>> contextFactory
        )
            : base(contextFactory)
        {

            CreateMap<BannerDto, Banner>()
                //.BeforeMap((src, dest) =>
                //{
                //    if (src.isNew)
                //        src.key = Guid.NewGuid().ToString();
                //})
                //.ForMember(dest => dest._Base64, opt => opt.MapFrom(src => src.base64Img))
                //.ForMember(dest => dest.ImagemUrl, opt =>
                //{
                //    opt.PreCondition(src => src.isNew && !String.IsNullOrEmpty(src.base64Img) && !String.IsNullOrEmpty(src.extensaoImg));
                //    opt.MapFrom((src, dest, member, ctx) =>
                //    {
                //        return dest.GetS3Url(((Usuario)Context.User)?.Prefeitura?.S3BaseUrl, src.extensaoImg);
                //    });
                //})
                //.ForMember(dest => dest.ImgHashCode, opt =>
                //{
                //    opt.PreCondition((src, ctx) => !string.IsNullOrEmpty(src.base64Img) && src.base64Img.Length >= 128);
                //    opt.MapFrom(src => src.base64Img.Substring(0, 128).GetHashCode());
                //})
                .IncludeBase<S3FileGenericDto, S3FileGeneric>()
                .ForMember(dest => dest.RotaInterna, opt =>
                {
                    opt.PreCondition(src => src.rotaTipoEnum == RotaTipoEnum.Link_Interno);
                    opt.MapFrom((src, dest, member, ctx) =>
                    {
                        RotaInterna rotaInterna = BannerService.FindRotaById(src.rotaInternaId);
                        return rotaInterna;
                    });
                })
                .ForMember(dest => dest.Path, opt => opt.PreCondition(src => src.rotaTipoEnum == RotaTipoEnum.Link_Externo))
                .ForMember(dest => dest.DhUltimoUpdate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UltimoEditor, opt => opt.MapFrom(src => UsuarioService.Obter(((Usuario)Context.User).Key, null)));
        }

    }
}