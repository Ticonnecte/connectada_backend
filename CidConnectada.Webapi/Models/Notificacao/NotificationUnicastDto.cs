using System.ComponentModel.DataAnnotations;
using CidConnectada.Webapi.Models.Notificacao;

namespace CidConnectada.Webapi.Models.Noticias
{
    public class NotificationUnicastDto : NotificationDto
    {
        [Required]
        public int usuarioId { get; set; }
        public string usuarioNome { get; set; }
    }
}