using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Notificacao
{
    public class ExpoNotificationError : BaseEntity<ExpoNotificationErrorKey>, IEquatable<ExpoNotificationError>
    {
        [IgnoreMap]
        public override ExpoNotificationErrorKey Key
        {
            get => new ExpoNotificationErrorKey { ExpoNotificationTokenId = ExpoNotificationTokenId, Code = Code };
        }

        [Required]
        public int ExpoNotificationTokenId { get; set; }
        [Required]
        public string Code { get; set; }
        public string Message { get; set; }

        public ExpoNotificationToken ExpoNotificationToken { get; set; }

        public bool Equals(ExpoNotificationError other)
        {
            bool result;
            if (ReferenceEquals(other, null))
            {
                result = false;
            }
            else if (ReferenceEquals(other, this))
            {
                result = true;
            }
            else
            {
                result = EntityUtil.EqualsEntity(this, other);
            }

            return result;
        }
    }
}