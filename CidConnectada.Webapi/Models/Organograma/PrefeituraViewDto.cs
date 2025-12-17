using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Organograma
{
    public class PrefeituraViewDto : BaseEntityModel<int>
    {
        [Required(AllowEmptyStrings = false)]
        public string nome { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string dominio { get; set; }
        
        public int enderecoId { get; set; }
        public string enderecoCompleto { get; set; }
        
        public string bucketName { get; set; }
        public string zApiIdInstancia { get; set; }
        public string zApiToken { get; set; }
        public string zApiClientToken { get; set; }

        public string googleMapsApiKey { get; set; }

        public string primaryMainColor { get; set; }
        public string primaryDarkColor { get; set; }
        public string primaryLightColor { get; set; }
        public string secondaryMainColor { get; set; }
        public string secondaryDarkColor { get; set; }
        public string secondaryLightColor { get; set; }

        public string logoHeaderUrl { get; set; }
        public string logoHorizontalUrl { get; set; }
        public string logoVerticalUrl { get; set; }
        
    }
}
