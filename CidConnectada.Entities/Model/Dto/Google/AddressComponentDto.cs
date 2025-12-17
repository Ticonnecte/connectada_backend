using System.Collections.Generic;

namespace CidConnectada.Entities.Model.Dto.Google
{
    public class AddressComponentDto
    {
        public AddressComponentDto()
        {
            IList<string> types = new List<string>();
        }
        public string long_name { get; set; }
        public string short_name { get; set; }
        public IList<string> types { get; set; }
    }
}