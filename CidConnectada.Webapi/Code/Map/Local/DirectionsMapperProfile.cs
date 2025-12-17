using AutoMapper;
using CidConnectada.Entities.Model.Dto.Google;
using CidConnectada.Entities.Model.Dto.Location;

namespace CidConnectada.Webapi.Code.Map.Local
{
    public class DirectionsMapperProfile : Profile
    {
        public DirectionsMapperProfile()
        {

            #region Dto -> Dto

            CreateMap<PlaceDto, EnderecoDto>()
                .AfterMap<GeoCodeDtoToAddressDtoAction>();

            #endregion

        }
    }

    public class GeoCodeDtoToAddressDtoAction : IMappingAction<PlaceDto, EnderecoDto>
    {
        public void Process(PlaceDto src, EnderecoDto dest)
        {
            foreach (var addressComponent in src.address_components)
            {
                dest.rua = addressComponent.types.Contains("route") ? addressComponent.long_name : dest.rua;
                dest.numero = addressComponent.types.Contains("street_number") ? addressComponent.long_name : dest.numero;
                dest.bairro = addressComponent.types.Contains("locality")
                    || addressComponent.types.Contains("sublocality") ? addressComponent.long_name : dest.bairro;
                dest.cidadeNome = addressComponent.types.Contains("administrative_area_level_2") ? addressComponent.long_name : dest.cidadeNome;
                dest.estadoSigla = addressComponent.types.Contains("administrative_area_level_1") ? addressComponent.short_name : dest.estadoSigla;
                dest.cep = addressComponent.types.Contains("postal_code") ? addressComponent.long_name : dest.cep;
            }
            ;

            dest.complemento = src.name;
            dest.coordenadas = src.geometry.location;
            dest.googleMapsPlaceId = src.place_id;
        }
    }
}