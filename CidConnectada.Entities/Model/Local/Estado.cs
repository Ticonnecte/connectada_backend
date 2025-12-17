using System;
using System.Collections.Generic;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Local
{
    public class Estado : BaseEntity<int>, IEquatable<Estado>
    {
        public string Nome { get; set; }
        public string Sigla { get; set; }
        public ISet<Cidade> CidadeSet { get; set; }

        public bool Equals(Estado other)
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