using System;
using AutoMapper;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Saude
{
    public class UbsEspecialidadeMedica : BaseEntity<UbsEspecialidadeMedicaKey>, IEquatable<UbsEspecialidadeMedica>
    {
        [IgnoreMap]
        public override UbsEspecialidadeMedicaKey Key => new UbsEspecialidadeMedicaKey
        {
            UbsId = UbsId,
            EspecMedId = EspecMedId
        };

        public string UbsId { get; set; }
        public int EspecMedId { get; set; }
        public UnidadeBasicaSaude UnidadeBasicaSaude { get; set; }
        public EspecialidadeMedica EspecialidadeMedica { get; set; }

        public bool Equals(UbsEspecialidadeMedica other)
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