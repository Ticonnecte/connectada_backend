using CidConnectada.Webapi.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Infos
{
    public class InfoDto : HtmlContentDto
    {
        public InfoDto()
            : base()
        {
        }
        public InfoDto(bool isView)
            : base(isView)
        {
        }

        public int categoriaId {  get; set; }
        public string categoriaNome { get; set; }
    }

}
