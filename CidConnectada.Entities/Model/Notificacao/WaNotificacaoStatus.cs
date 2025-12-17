using AutoMapper;
using CidConnectada.Entities.Model.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zenite.Pi.Entities;

namespace CidConnectada.Entities.Model.Notificacao
{
    public class WaNotificacaoStatus: BaseEntity<NotificationUserKey>
    {
        [IgnoreMap]
        public override NotificationUserKey Key
        {
            get => new NotificationUserKey { UsuarioId = UsuarioId, NotificationId = NotificationId };
        }

        [Required]
        public int UsuarioId { get; set; }
        [Required]
        public int NotificationId { get; set; }
        
        public string ZaapId { get; set; }
        public string MessageId { get; set; }

        public DateTime SentAt { get; set; }
        public DateTime ReceivedAt { get; set; }
        public DateTime ReadAt { get; set; }
        
        public Notification Notification { get; set; }
        public Usuario Usuario { get; set; }

    }
}
