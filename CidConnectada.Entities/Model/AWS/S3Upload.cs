using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CidConnectada.Entities.Model.AWS
{
    public class S3Upload
    {
        public string Key { get; set; }
        public string Base64 { get; set; }
        public bool Remove { get; set; } = false;
    }
}
