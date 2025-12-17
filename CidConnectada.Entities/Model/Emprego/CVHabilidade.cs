using System;
using AutoMapper;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Emprego
{
    public class CVHabilidade : BaseEntity<CVHabilidadeKey>, IEquatable<CVHabilidade>
    {
        [IgnoreMap]
        public override CVHabilidadeKey Key => new CVHabilidadeKey
        {
            CVId = CVId,
            HabilidadeId = HabilidadeId
        };

        public int CVId { get; set; }
        public int HabilidadeId { get; set; }

        public CurriculumVitae CurriculumVitae { get; set; }
        public Habilidade Habilidade { get; set; }

        public bool Equals(CVHabilidade other)
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