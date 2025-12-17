using System;
using System.Collections.Generic;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Emprego
{
    public class Competencia : EmpregoDetail, IEquatable<Competencia>
    {
        public ISet<OfertaVagaCompetencia> OfertaVagaCompetenciaSet { get; set; } = new HashSet<OfertaVagaCompetencia>();
        public ISet<CVCompetencia> CVCompetenciaSet { get; set; } = new HashSet<CVCompetencia>();

        public bool Equals(Competencia other)
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