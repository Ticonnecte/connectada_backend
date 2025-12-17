using System.ComponentModel.DataAnnotations;
using CidConnectada.Entities.Model.Enums;

namespace CidConnectada.Webapi.Models.Notificacao
{
    public class NotificationDto : NotificationBaseDto
    {
        public string subTitle { get; set; }
        [Required]
        public string body { get; set; }
        //public string dataJson { get; set; }
    }
}