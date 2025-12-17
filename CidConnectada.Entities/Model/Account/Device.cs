using System;
using System.Collections.Generic;
using CidConnectada.Entities.Model.Notificacao;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Account
{
    public class Device : BaseEntity<Guid>, IEquatable<Device>
    {
        public override Guid Key { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public ISet<RefreshToken> RefreshTokenSet { get; set; } = new HashSet<RefreshToken>();
        public ISet<ExpoNotificationToken> ExpoNotificationTokenSet { get; set; } = new HashSet<ExpoNotificationToken>();

        public bool Equals(Device other)
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