using System;
using System.Collections.Generic;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Emprego
{
    public class FaixaSalarial : BaseEntity<int>, IEquatable<FaixaSalarial>
    {
        public decimal? ValorMin { get; set; }
        public decimal? ValorMax { get; set; }

        public ISet<OfertaVaga> OfertaVagaSet { get; set; } = new HashSet<OfertaVaga>();

        public bool Equals(FaixaSalarial other)
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