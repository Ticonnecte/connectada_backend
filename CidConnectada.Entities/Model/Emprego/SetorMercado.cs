using System;
using System.Collections.Generic;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Emprego
{
    public class SetorMercado : EmpregoDetail, IEquatable<SetorMercado>
    {
        public ISet<OfertaVaga> OfertaVagaSet { get; set; } = new HashSet<OfertaVaga>();
        public ISet<CurriculumVitae> CurriculumVitaeSet { get; set; } = new HashSet<CurriculumVitae>();

        public bool Equals(SetorMercado other)
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