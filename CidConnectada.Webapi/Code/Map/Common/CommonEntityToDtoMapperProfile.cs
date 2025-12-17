using CidConnectada.Entities.Model.Dto.Location;
using System;
using System.Data.Entity.Spatial;
using Zenite.Pi.Context;

namespace CidConnectada.Webapi.Code.Map.Common
{
    public class CommonEntityToDtoMapperProfile : EntityToDtoBaseProfile
    {
        public CommonEntityToDtoMapperProfile(Func<ContextRequest<int, string>> contextFactory)
            : base(contextFactory)
        {
            CreateMap<DbGeography, LocationDto>()
                .ForMember(dest => dest.lat, opt => opt.MapFrom(src => src.Latitude))
                .ForMember(dest => dest.lng, opt => opt.MapFrom(src => src.Longitude));
        }
    }
}