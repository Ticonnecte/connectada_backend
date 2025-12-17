using CidConnectada.Entities.Model.Enums;
using CidConnectada.Webapi.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace CidConnectada.Webapi.Models.Relacionamento
{
    public class DialogoSimpleDto : S3FileGenericDto
    {
        public string titulo { get; set; }
        public string dialogoStatusEnumNome { get; set; }

        public string imagemUrl
        {
            get {
                return _imgUrl;
            }
            set {
                _imgUrl = value;
            }
        }
    }
    
    public class DialogoBaseDto : S3FileGenericDto
    {
        [Required]
        public string titulo { get; set; }
        public string descricao { get; set; }
        [Required]
        public DialogoAssuntoEnum assuntoDialogoEnum { get; set; }
        [Required]
        public string secretariaId { get; set; }
        public bool isAnonymous { get; set; }
    }
    
    public class DialogoDto : DialogoBaseDto
    {
        public string extensaoImg
        {
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
        public long enderecoId { get; set; }
    }
}