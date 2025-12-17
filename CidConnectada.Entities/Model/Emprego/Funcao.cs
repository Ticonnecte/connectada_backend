using System;
using System.Collections.Generic;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Emprego
{
    public class Funcao : EmpregoDetail, IEquatable<Funcao>
    {
        public ISet<OfertaVaga> OfertaVagaSet { get; set; } = new HashSet<OfertaVaga>();
        public ISet<CurriculumVitae> CurriculumVitaeSet { get; set; } = new HashSet<CurriculumVitae>();
        public ISet<CVExperiencia> CVExperienciaSet { get; set; } = new HashSet<CVExperiencia>();

        public bool Equals(Funcao other)
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