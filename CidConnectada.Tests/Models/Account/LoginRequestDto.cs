using System.Collections.Generic;

namespace CidConnectada.Tests.Models
{
    public class LoginRequestDto
    {
        public string username { get; set; }
        public string password { get; set; }
        public string grant_type { get; set; }
        public string device_id { get; set; }
        public string device_name { get; set; }
        public string device_type { get; set; }

        public IDictionary<string, string> ToKeyValuePairs()
        {
            IDictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            foreach (var prop in GetType().GetProperties())
            {
                keyValuePairs.Add(prop.Name, prop.GetValue(this)?.ToString());
            }
            return keyValuePairs;
        }
    }
}