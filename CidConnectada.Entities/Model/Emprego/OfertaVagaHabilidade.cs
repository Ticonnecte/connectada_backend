using System;
using AutoMapper;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Emprego
{
    public class OfertaVagaHabilidade : BaseEntity<OfertaVagaHabilidadeKey>, IEquatable<OfertaVagaHabilidade>
    {
        [IgnoreMap]
        public override OfertaVagaHabilidadeKey Key => new OfertaVagaHabilidadeKey
        {
            OfertaVagaId = OfertaVagaId,
            HabilidadeId = HabilidadeId
        };

        public long OfertaVagaId { get; set; }
        public int HabilidadeId { get; set; }

        public OfertaVaga OfertaVaga { get; set; }
        public Habilidade Habilidade { get; set; }

        public bool Equals(OfertaVagaHabilidade other)
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