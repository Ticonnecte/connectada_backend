using System;
using System.Collections.Generic;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Emprego
{
    public class Habilidade : EmpregoDetail, IEquatable<Habilidade>
    {
        public ISet<OfertaVagaHabilidade> OfertaVagaHabilidadeSet { get; set; } = new HashSet<OfertaVagaHabilidade>();
        public ISet<CVHabilidade> CVHabilidadeSet { get; set; } = new HashSet<CVHabilidade>();

        public bool Equals(Habilidade other)
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