using System;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Account
{
    public class VerificacaoConta : BaseEntity<Guid>, IEquatable<VerificacaoConta>
    {
        public Usuario Usuario { get; set; }
        public string Codigo { get; set; }
        public DateTime DataExpiracaoUtc { get; set; }

        public bool Equals(VerificacaoConta other)
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