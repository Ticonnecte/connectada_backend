using System;
using System.Collections.Generic;
using CidConnectada.Entities.Model.Account;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Notificacao
{
    public class ExpoNotificationToken : BaseEntity<int>, IEquatable<ExpoNotificationToken>
    {
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Usuario User { get; set; }
        public Device Device { get; set; }

        public ISet<ExpoNotificationStatus> ExpoNotificationStatusSet { get; set; } = new HashSet<ExpoNotificationStatus>();
        public ISet<ExpoNotificationError> ExpoNotificationErrorSet { get; set; } = new HashSet<ExpoNotificationError>();

        public bool Equals(ExpoNotificationToken other)
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