using System;
using System.Collections.Generic;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Notificacao
{
    public class NotificationMulticast : Notification, IEquatable<NotificationMulticast>
    {
        public ISet<NotificationMulticastUser> NotificationMulticastUserSet { get; set; } = new HashSet<NotificationMulticastUser>();

        public bool Equals(NotificationMulticast other)
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