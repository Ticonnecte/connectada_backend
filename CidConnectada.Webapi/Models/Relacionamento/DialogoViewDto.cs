using System;
using CidConnectada.Entities.Model.Dto.Location;
using CidConnectada.Entities.Model.Enums;

namespace CidConnectada.Webapi.Models.Relacionamento
{
    public class DialogoViewDto : DialogoBaseDto
    {
        public DateTime dhCriacao { get; set; }
        public string imagemUrl
        {
            get {
                return _imgUrl;
            }
            set {
                _imgUrl = value;
            }
        }
        public DateTime? dataPrevistaExecuacao { get; set; }
        public DateTime? dataPrevistaFinalizacao { get; set; }
        public string assuntoDialogoEnumNome { get; set; }
        public DialogoStatusEnum dialogoStatusEnum { get; set; }
        public string dialogoStatusEnumNome { get; set; }
        public EnderecoDto endereco { get; set; }
        public string secretariaNome { get; set; }
        public int cidadaoId { get; set; }
        public string cidadaoNome { get; set; }
    }
}