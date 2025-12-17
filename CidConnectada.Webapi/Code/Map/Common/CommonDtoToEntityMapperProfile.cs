using CidConnectada.Entities.Model.Dto.Location;
using System;
using System.Data.Entity.Spatial;
using Zenite.Pi.Context;

namespace CidConnectada.Webapi.Code.Map.Common
{
    // Dto => Entity
    public class CommonDtoToEntityMapperProfile : DtoToEntityBaseProfile
    {
        public CommonDtoToEntityMapperProfile(Func<ContextRequest<int, string>> contextFactory)
            :base(contextFactory)
        {
            CreateMap<LocationDto, DbGeography>()
                .ConvertUsing(src => DbGeography.FromText(src.ToWkt(true)));
        }
    }
}