using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zenite.Pi.Entities;
using Zenite.Pi.Entities.Model.MultiTenancy;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Account
{
    public class AspNetUsers : MultiTenancy<string, int>, IEquatable<AspNetUsers>
    {
        public string Email { get; set; }
        [Required]
        public bool Emailconfirmed { get; set; }
        public string Passwordhash { get; set; }
        public string Securitystamp { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public bool Phonenumberconfirmed { get; set; }
        [Required]
        public bool Twofactorenabled { get; set; }
        public DateTime? Lockoutenddateutc { get; set; }
        [Required]
        public bool Lockoutenabled { get; set; }
        [Required]
        public int Accessfailedcount { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Username { get; set; }


        public virtual Usuario User { get; set; }
        public virtual ISet<AspNetUserRoles> AspNetUserRolesSet { get; set; }

        public bool Equals(AspNetUsers other)
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