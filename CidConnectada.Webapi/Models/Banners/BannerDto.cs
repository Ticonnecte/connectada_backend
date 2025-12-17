using CidConnectada.Entities.Model.Enums;
using CidConnectada.Webapi.Models.Common;
using System.ComponentModel.DataAnnotations;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Banners
{
    public class BannerDto : S3FileGenericDto
    {
        [Required]
        public string nome { get; set; }
        public string descricao { get; set; }
        public bool estaNaHome { get; set; }
        [Required]
        public RotaTipoEnum rotaTipoEnum { get; set; }
        [RequiredIfPropertyEquals("rotaTipoEnum", "Link_Interno")]
        public int rotaInternaId { get; set; }
        [RequiredIfPropertyEquals("rotaTipoEnum", "Link_Externo")]
        public string path { get; set; }
        public string extensaoImg {
            get {
                return _extensao;
            }
            set {
                _extensao = value;
            }
        }
        public string base64Img {
            get {
                return _base64;
            }
            set {
                _base64 = value;
            }
        }
    }
}