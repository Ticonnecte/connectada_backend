using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CidConnectada.Entities.Model.Local;

namespace CidConnectada.Webapi.Models.Saude
{
    public class UbsDto : UbsBaseDto
    {
        [Required]
        public long? enderecoId { get; set; }
        public decimal? areaTotal { get; set; }
        public int? numeroSalas { get; set; }
        public int? numEquipeSaudeFamilia { get; set; }
        public int? numProfissionais { get; set; }
        public string responsavelNome { get; set; }
        private string _responsavelWhatsApp { get; set; }

        public string imagemUrl
        {
            get {
                return _imgUrl;
            }
            set {
                _imgUrl = value;
            }
        }

        public string responsavelWhatsApp
        {
            get => _responsavelWhatsApp;
            set => _responsavelWhatsApp = String.IsNullOrEmpty(value) ? "" : String.Concat(value.Where(Char.IsDigit));
        }

        public string vinculacaoAdmnistrativa { get; set; }
        public string extensaoImg {
            get {
                return _extensao;
            }
            set {
                _extensao = value;
            }
        }
        public string base64Img
        {
            get {
                return _base64;
            }
            set {
                _base64 = value;
            }
        }

    }
}
