using System;
using CidConnectada.Entities.Model.Banners;
using CidConnectada.Entities.Model.Enums;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Organograma
{
    public class SecretariaMenu : BaseEntity<SecretariaMenuKey>, IEquatable<SecretariaMenu>

    {
        public override SecretariaMenuKey Key
        {
            get => new SecretariaMenuKey { SecretariaId = SecretariaId, OrdemIdx = OrdemIdx };
        }

        public string SecretariaId { get; set; }
        public byte OrdemIdx { get; set; }
        public string IconeNome { get; set; }
        public string Titulo { get; set; }
        public bool IsActive { get; set; }
        public RotaTipoEnum RotaTipoEnum { get; set; }
        public string Path { get; set; }
        public RotaInterna RotaInterna { get; set; }
        public Secretaria Secretaria { get; set; }

        public bool Equals(SecretariaMenu other)
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