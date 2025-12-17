using System;
using Zenite.Pi.Entities;

namespace CidConnectada.Entities.Model.Account
{
    public class AspNetUserRolesKey : IEntityKey
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }

        public override bool Equals(object obj)
        {
            return obj is AspNetUserRolesKey && GetHashCode() == obj.GetHashCode();
        }

        public override string ToString()
        {
            return RoleId + UserId;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public int CompareTo(object obj)
        {
            int result;
            if (obj is AspNetUserRolesKey)
            {
                result = UserId.CompareTo(((AspNetUserRolesKey)obj).UserId);
                if (result == 0)
                {
                    result = RoleId.CompareTo(((AspNetUserRolesKey)obj).RoleId);
                }
            }
            else
            {
                throw new TypeInitializationException(obj.GetType().FullName, null);
            }
            return result;
        }

        public object[] ToArray()
        {
            return new object[2] { UserId, RoleId };
        }
    }
}