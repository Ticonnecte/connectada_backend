using System.Collections.Generic;

namespace CidConnectada.Entities.Model.Dto.Google
{
    public class PlaceDto
    {
        public PlaceDto()
        {
            IList<AddressComponentDto> address_components = new List<AddressComponentDto>();
            IList<string> types = new List<string>();
        }
        public IList<AddressComponentDto> address_components { get; set; }
        public string formatted_address { get; set; }
        public string name { get; set; }
        public GeometryDto geometry { get; set; }
        public string place_id { get; set; }
        public IList<string> types { get; set; }
    }
}