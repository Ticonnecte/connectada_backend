using System.Collections.Generic;

namespace CidConnectada.Webapi.Models.Account
{
    public class PermissionsDto
    {
        public Dictionary<string, Dictionary<string, bool>> permissions { get; set; }
    }
}
