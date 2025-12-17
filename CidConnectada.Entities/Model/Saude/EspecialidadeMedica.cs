using System;
using System.Collections.Generic;
using CidConnectada.Entities.Model.Noticias;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Saude
{
    public class EspecialidadeMedica : BaseEntity<int>, IEquatable<Noticia>
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }

        public ISet<UbsEspecialidadeMedica> UbsEspecialidadeMedicaSet { get; set; }

        public bool Equals(Noticia other)
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