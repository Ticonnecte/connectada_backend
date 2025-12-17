using System;
using AutoMapper;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Emprego
{
    public class CVCompetencia : BaseEntity<CVCompetenciaKey>, IEquatable<CVCompetencia>
    {
        [IgnoreMap]
        public override CVCompetenciaKey Key => new CVCompetenciaKey
        {
            CVId = CVId,
            CompetenciaId = CompetenciaId
        };

        public int CVId { get; set; }
        public int CompetenciaId { get; set; }

        public CurriculumVitae CurriculumVitae { get; set; }
        public Competencia Competencia { get; set; }

        public bool Equals(CVCompetencia other)
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