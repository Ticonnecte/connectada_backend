using System;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Account
{
    public class AspNetUserRoles : BaseEntity<AspNetUserRolesKey>, IEquatable<AspNetUserRoles>
    {
        public override AspNetUserRolesKey Key
        {
            get
            {
                return new AspNetUserRolesKey() { UserId = this.UserId, RoleId = this.RoleId };
            }
        }

        public string UserId { get; set; }
        public string RoleId { get; set; }

        public AspNetUsers AspNetUsers { get; set; }
        public AspNetRoles AspNetRoles { get; set; }

        public bool Equals(AspNetUserRoles other)
        {
            bool result;
            if (Object.ReferenceEquals(other, null))
            {
                result = false;
            }
            else if (Object.ReferenceEquals(other, this))
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
