using CidConnectada.Entities.Model.Local;
using CidConnectada.Entities.Model.Organograma;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenite.Pi.Entities.Model.Account;
using Zenite.Pi.Entities.Model.MultiTenancy;
using Zenite.Pi.Util.Control;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Organograma
{
    public class RedesSociaisDto
    {
        public string facebook { get; set; }
        public string youtube { get; set; }
        public string instagram { get; set; }
        public string site { get; set; }
    }
}
