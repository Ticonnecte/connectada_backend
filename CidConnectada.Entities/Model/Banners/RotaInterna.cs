using System;
using System.Collections.Generic;
using CidConnectada.Entities.Model.Organograma;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Banners
{
    public class RotaInterna : BaseEntity<int>, IEquatable<RotaInterna>
    {
        public string Nome { get; set; }
        public string Path { get; set; }
        public bool EhBanner { get; set; }
        public bool EhSecretaria { get; set; }
        public virtual ISet<SecretariaMenu> SecretariaMenuSet { get; set; } = new HashSet<SecretariaMenu>();
        public ISet<Banner> BannerSet { get; set; } = new HashSet<Banner>();

        public bool Equals(RotaInterna other)
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