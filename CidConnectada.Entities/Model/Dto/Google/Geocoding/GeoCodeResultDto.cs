using System.Collections.Generic;

namespace CidConnectada.Entities.Model.Dto.Google.Geocoding
{
    public class GeoCodeResultDto
    {
        public string status;
        public GeoCodeResultDto()
        {
            IList<PlaceDto> results = new List<PlaceDto>();
        }
        public IList<PlaceDto> results { get; set; }
    }
}