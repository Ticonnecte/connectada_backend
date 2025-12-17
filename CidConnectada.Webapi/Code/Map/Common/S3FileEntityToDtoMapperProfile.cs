using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Dto.Location;
using CidConnectada.Entities.Model.Infos;
using CidConnectada.Webapi.Models.Common;
using System;
using System.Data.Entity.Spatial;
using Zenite.Pi.Context;

namespace CidConnectada.Webapi.Code.Map.Common
{
    public class S3FileEntityToDtoMapperProfile : EntityToDtoBaseProfile
    {
        public S3FileEntityToDtoMapperProfile(Func<ContextRequest<int, string>> contextFactory)
            :base(contextFactory)
        {
            CreateMap<HtmlContent, HtmlContentDto>()
                .IncludeBase<S3FileGeneric, S3FileGenericDto>();

            CreateMap<S3FileGeneric, S3FileGenericDto>()
                .AfterMap((src, dest) => {
                    dest.SetImgUrl(src._ImgUrl);
                });

        }
    }
}