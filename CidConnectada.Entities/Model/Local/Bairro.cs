using System;
using System.Collections.Generic;
using CidConnectada.Entities.Model.Account;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Local
{
    public class Bairro : BaseEntity<int>, IEquatable<Bairro>
    {
        public string Nome { get; set; }
        public Cidade Cidade { get; set; }
        public ISet<Cidadao> CidadaoSet { get; set; } = new HashSet<Cidadao>();
        public bool Equals(Bairro other)
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