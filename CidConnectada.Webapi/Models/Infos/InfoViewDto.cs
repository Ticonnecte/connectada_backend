using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CidConnectada.Webapi.Models.Infos
{
    public class InfoViewDto : CategoriaDto
    {
        public IList<InfoDto> infoList { get; set; }
    }
}
