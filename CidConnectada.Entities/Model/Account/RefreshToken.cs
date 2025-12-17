using System;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Account
{
    public class RefreshToken : BaseEntity<Guid>, IEquatable<RefreshToken>
    {
        public Device Device { get; set; }
        public Usuario User { get; set; }
        public string UserAgent { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public string ProtectedTicket { get; set; }

        public bool Equals(RefreshToken other)
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