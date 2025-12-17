using System;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Notificacao
{
    public class NotificationBroadcast : Notification, IEquatable<NotificationBroadcast>
    {
        public bool Equals(NotificationBroadcast other)
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