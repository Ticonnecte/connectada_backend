using System;
using AutoMapper;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Emprego
{
    public class CVExperiencia : BaseEntity<CVExperienciaKey>, IEquatable<CVExperiencia>
    {
        [IgnoreMap]
        public override CVExperienciaKey Key => new CVExperienciaKey
        {
            CVId = CVId,
            ItemIndex = ItemIndex
        };
        public int CVId { get; set; }
        public byte ItemIndex { get; set; }

        public string NomeEmpresa { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime? PeriodoFinal { get; set; }

        public string Atividades { get; set; }
        public Funcao Funcao { get; set; }
        public CurriculumVitae CurriculumVitae { get; set; }

        public bool Equals(CVExperiencia other)
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