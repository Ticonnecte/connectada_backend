using System;
using AutoMapper;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Emprego
{
    public class OfertaVagaCompetencia : BaseEntity<OfertaVagaCompetenciaKey>, IEquatable<OfertaVagaCompetencia>
    {
        [IgnoreMap]
        public override OfertaVagaCompetenciaKey Key => new OfertaVagaCompetenciaKey
        {
            OfertaVagaId = OfertaVagaId,
            CompetenciaId = CompetenciaId
        };

        public long OfertaVagaId { get; set; }
        public int CompetenciaId { get; set; }

        public OfertaVaga OfertaVaga { get; set; }
        public Competencia Competencia { get; set; }

        public bool Equals(OfertaVagaCompetencia other)
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