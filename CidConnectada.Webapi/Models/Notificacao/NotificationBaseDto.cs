using System;
using System.ComponentModel.DataAnnotations;
using CidConnectada.Entities.Model.Enums;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Notificacao
{
    public class NotificationBaseDto : BaseEntityModel<int>
    {
        [Required]
        public string title { get; set; }
        [RequiredIfPropertyEquals(nameof(destinoEnum), nameof(NotificationDestinyEnum.Push), nameof(NotificationDestinyEnum.Both))]
        public NotificationPriorityEnum? prioridadeEnum { get; set; }
        public string prioridadeEnumNome { get; set; }
        public NotificationStatusEnum statusEnum { get; set; }
        public string statusEnumNome { get; set; }
        [Required]
        public NotificationDestinyEnum destinoEnum { get; set; }
        public string destinoEnumNome { get; set; }
        public NotificationTypeEnum tipoEnum { get; set; }
        public string tipoEnumNome { get; set; }
        [NotPastDate]
        public DateTime? dhAgendamento { get; set; }
    }
}