using CidConnectada.Entities.Model.Local;
using CidConnectada.Entities.Model.Organograma;
using CidConnectada.Webapi.Models.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenite.Pi.Entities.Model.Account;
using Zenite.Pi.Entities.Model.MultiTenancy;
using Zenite.Pi.Util.Control;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Organograma
{
    public class PrefeituraDto : BaseEntityModel<int>
    {
        [Required(AllowEmptyStrings = false)]
        public string nome { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        [RegularExpression("^[a-zA-Z0-9-]*$", ErrorMessage = "O domínio deve conter apenas caracteres alfanuméricos (letras e números).")]
        public string dominio { get; set; }
        //[Required]
        public int enderecoId { get; set; }
        public string enderecoCompleto { get; set; }

        [Required]
        public UsuarioDto admin { get; set; }

        #region AWS
        //public string bucketName { get; set; }
        public string s3Region { get; set; }
        public string s3AccessKeyId { get; set; }
        public string s3AccessKeySecret { get; set; }

        #endregion

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
        
        public string facebook { get; set; }
        public string youtube { get; set; }
        public string instagram { get; set; }
        public string site { get; set; }

        public string extensaoLogoHeader { get; set; }
        public string base64LogoHeader { get; set; }
        public string extensaoLogoHorizontal { get; set; }
        public string base64LogoHorizontal { get; set; }
        public string extensaoLogoVertical { get; set; }
        public string base64LogoVertical { get; set; }
        
        public string logoHeaderUrl { get; set; }
        public string logoHorizontalUrl { get; set; }
        public string logoVerticalUrl { get; set; }

    }
}
