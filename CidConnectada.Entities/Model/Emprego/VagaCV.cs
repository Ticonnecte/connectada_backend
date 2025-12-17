using System;
using AutoMapper;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Emprego
{
    public class VagaCV : BaseEntity<VagaCVKey>, IEquatable<VagaCV>
    {
        [IgnoreMap]
        public override VagaCVKey Key => new VagaCVKey
        {
            OfertaVagaId = OfertaVagaId,
            CVId = CVId
        };

        public long OfertaVagaId { get; set; }
        public int CVId { get; set; }
        public decimal GrauCorrelacao { get; set; }
        public bool InteresseCandidato { get; set; }
        public bool InteresseEmpregador { get; set; }

        public OfertaVaga OfertaVaga { get; set; }
        public CurriculumVitae CurriculumVitae { get; set; }

        public bool Equals(VagaCV other)
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