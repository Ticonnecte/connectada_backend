using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using CidConnectada.Entities.Model.Account;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Notificacao
{
    public class NotificationMulticastUser : BaseEntity<NotificationUserKey>, IEquatable<NotificationMulticastUser>
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
        public Usuario Usuario { get; set; }
        public NotificationMulticast NotificationMulticast { get; set; }

        public bool Equals(NotificationMulticastUser other)
        {
            bool result;
            if (ReferenceEquals(other, null))
                result = false;
            else if (ReferenceEquals(other, this))
                result = true;
            else
                result = EntityUtil.EqualsEntity(this, other);

            return result;
        }
    }
}