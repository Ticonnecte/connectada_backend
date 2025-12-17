using System;
using System.Collections.Generic;
using CidConnectada.Entities.Model.Emprego;
using CidConnectada.Entities.Model.Local;
using CidConnectada.Entities.Model.Relacionamento;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Account
{
    public class Cidadao : Usuario, IEquatable<Cidadao>
    {
        public Bairro Bairro { get; set; }
        public ISet<OfertaVaga> OfertaVagaSet { get; set; } = new HashSet<OfertaVaga>();
        public ISet<CurriculumVitae> CurriculumVitaeSet { get; set; } = new HashSet<CurriculumVitae>();
        public ISet<Dialogo> DialogoSet { get; set; } = new HashSet<Dialogo>();
        public ISet<Comercios.Comercio> ComercioSet { get; set; } = new HashSet<Comercios.Comercio>();
        public bool Equals(Cidadao other)
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