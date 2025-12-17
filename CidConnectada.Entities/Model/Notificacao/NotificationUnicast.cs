using CidConnectada.Entities.Model.Account;
using System;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Notificacao
{
    public class NotificationUnicast : Notification, IEquatable<NotificationUnicast>
    {
        public Usuario Usuario { get; set; }
        public bool Equals(NotificationUnicast other)
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