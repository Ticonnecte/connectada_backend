using System;
using CidConnectada.Entities.Model.Enums;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Notificacao
{
    public class ExpoNotificationStatus : BaseEntity<int>, IEquatable<ExpoNotificationToken>
    {
        public DateTime SentAt { get; set; }
        public string ExpoId { get; set; }
        public NotificationStatusEnum StatusEnum { get; set; }

        public Notification Notification { get; set; }
        public ExpoNotificationToken ExpoNotificationToken { get; set; }

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