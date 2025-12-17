using System;
using CidConnectada.Entities.Model.Account;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Comunicacao
{
    public class EnqueteResposta : BaseEntity<int>, IEquatable<EnqueteResposta>
    {
        public EnqueteOpcao EnqueteOpcao { get; set; }
        public Usuario Usuario { get; set; }

        public bool Equals(EnqueteResposta other)
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