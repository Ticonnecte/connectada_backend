using CidConnectada.Entities.Model.Dto.Location;
using CidConnectada.Entities.Model.Local;
using CidConnectada.Services.Intf.Local;
using System;
using Zenite.Pi.Context;

namespace CidConnectada.Webapi.Code.Map.Local
{
    // Dto => Entity
    public class LocationDtoToEntityMapperProfile : DtoToEntityBaseProfile
    {
        #region Services
        protected IEnderecoService EnderecoService => GetService<IEnderecoService>();


        #endregion
        public LocationDtoToEntityMapperProfile(Func<ContextRequest<int, string>> contextFactory
        )
            : base(contextFactory)
        {
            CreateMap<EnderecoDto, Endereco>()
                .ForMember(dest => dest.Coordenadas, opt => opt.MapFrom(src => src.coordenadas.ToDbGeography()))
                .ForMember(dest => dest.GoogleMapsPlaceId, opt => opt.MapFrom(src => src.googleMapsPlaceId))
                .ForMember(dest => dest.Cidade, opt => opt.MapFrom((src, dest, member, ctx) =>
                {
                    Cidade result = null;
                    if (src.cidadeId != 0)
                    {
                        result = EnderecoService.GetCidade(src.cidadeId);
                    }
                    else if (!String.IsNullOrWhiteSpace(src.cidadeNome) && !String.IsNullOrWhiteSpace(src.estadoSigla))
                    {
                        result = EnderecoService.GetCidade(src.cidadeNome, src.estadoSigla);
                    }
                    return result;
                }));
            //   EnderecoService = enderecoService;
        }

    }
}