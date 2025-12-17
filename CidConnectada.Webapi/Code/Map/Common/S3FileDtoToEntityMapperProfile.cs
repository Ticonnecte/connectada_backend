using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Dto.Location;
using CidConnectada.Entities.Model.Infos;
using CidConnectada.Webapi.Models.Common;
using System;
using System.Data.Entity.Spatial;
using System.Web.Razor.Generator;
using Zenite.Pi.Context;

namespace CidConnectada.Webapi.Code.Map.Common
{
    // Dto => Entity
    public class S3FileDtoToEntityMapperProfile : DtoToEntityBaseProfile
    {

        public S3FileDtoToEntityMapperProfile(Func<ContextRequest<int, string>> _contextFactory)
            : base(_contextFactory)
        {
            CreateMap<HtmlContentDto, HtmlContent>()
                .IncludeBase<S3FileGenericDto, S3FileGeneric>();

            CreateMap<S3FileGenericDto, S3FileGeneric>()
                .BeforeMap((src, dest) =>
                {
                    if (src.isNew)
                    {
                        src.key = Guid.NewGuid().ToString();
                        dest.Key = src.key;
                    }
                    else if (!string.IsNullOrEmpty(src.GetExtension()))
                    {
                        string oldExtension = dest.GetExtension();
                        if (oldExtension != src.GetExtension())
                        {
                            Context.CacheRequest.Add("OldExtension", oldExtension);
                        }
                    }
                })
                .ForMember(dest => dest._ImgUrl, opt =>
                {
                    opt.PreCondition((src, dest, ctx) => src.CanUpdate(dest.ImgHashCode.HasValue ? dest.ImgHashCode.Value : 0));
                    opt.MapFrom((src, dest, member, ctx) =>
                    {
                        return dest.GetS3Url(((Usuario)Context.User)?.Prefeitura?.S3BaseUrl, src.GetExtension());
                    });
                })
                .ForMember(dest => dest.ImgHashCode, opt =>
                {
                    opt.PreCondition((src, dest, ctx) => src.CanUpdate(dest.ImgHashCode.HasValue ? dest.ImgHashCode.Value : 0));
                    opt.MapFrom(src => src.CalculateHashCode());
                })
                .AfterMap((src, dest) =>
                {
                    dest._Base64 = src.GetBase64();
                });

        }
    }
}