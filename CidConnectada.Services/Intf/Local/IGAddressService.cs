using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Threading.Tasks;
using CidConnectada.Entities.Model.Dto.Location;
using Zenite.Pi.Services;

namespace CidConnectada.Services.Intf.Local
{
    public interface IGAddressService : IService
    {
        Task<EnderecoDto> GeoCode(EnderecoDto endereco);
        Task<EnderecoDto> GeoDecode(DbGeography location);
        Task<IList<PlaceAutoCompletePredictionsDto>> PlaceAutoComplete(PlaceAutoCompleteRequestDto model);
        Task<EnderecoDto> PlaceDetails(string placeId, string sessionToken = null);
    }
}