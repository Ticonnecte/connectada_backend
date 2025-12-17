using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Account
{
    public class AspNetRoles : BaseEntity<string>, IEquatable<AspNetRoles>
    {
        public AspNetRoles()
        {
            AspNetUserRolesSet = new HashSet<AspNetUserRoles>();
        }

        [Required()]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Permissions { get; set; }

        public virtual ISet<AspNetUserRoles> AspNetUserRolesSet { get; set; }

        public bool Equals(AspNetRoles other)
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
