using CidConnectada.Entities.Model.Dto.Location;
using CidConnectada.Entities.Model.Local;
using System;
using Zenite.Pi.Context;

namespace CidConnectada.Webapi.Code.Map.Local
{
    public class LocationEntityToDtoMapperProfile : EntityToDtoBaseProfile
    {
        public LocationEntityToDtoMapperProfile(Func<ContextRequest<int, string>> contextFactory)
            : base(contextFactory)
        {
            CreateMap<Endereco, EnderecoDto>()
                .ForMember(dest => dest.cidadeId, opt => opt.MapFrom(src => src.Cidade.Key))
                .ForMember(dest => dest.cidadeNome, opt => opt.MapFrom(src => Capitalize(src.Cidade.Nome)))
                .ForMember(dest => dest.estadoSigla, opt => opt.MapFrom(src => src.Cidade.Estado.Sigla));
        }
    }
}