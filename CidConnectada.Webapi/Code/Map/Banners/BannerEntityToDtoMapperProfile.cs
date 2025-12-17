using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Banners;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Webapi.Models.Banners;
using CidConnectada.Webapi.Models.Common;
using System;
using Zenite.Pi.Context;

namespace CidConnectada.Webapi.Code.Map.Banners
{
    // (Entity => Dto)
    public class BannerEntityToDtoMapperProfile : EntityToDtoBaseProfile
    {
        public BannerEntityToDtoMapperProfile(Func<ContextRequest<int, string>> contextFactory)
            : base(contextFactory)
        {

            CreateMap<Banner, BannerViewDto>()
                //.IncludeBase<S3FileGeneric, S3FileGenericDto>()
                .ForMember(dest => dest.path, opt => opt.MapFrom((src, dest, member, ctx) =>
                {
                    if (src.RotaTipoEnum == RotaTipoEnum.Link_Interno)
                    {
                        return src.RotaInterna.Path;
                    }
                    return src.Path;
                }))
                .ForMember(dest => dest.rotaTipoEnumNome, opt => opt.MapFrom(src => src.RotaTipoEnum.ToString().Replace("_", " ")))
                .ForMember(dest => dest.ultimoEditor, opt => opt.MapFrom(src => src.UltimoEditor.NomeCompleto));
        }
    }
}