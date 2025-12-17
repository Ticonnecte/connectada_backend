using System;
using System.Collections.Generic;
using CidConnectada.Entities.Model.Account;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Emprego
{
    public class CurriculumVitae : BaseEntity<int>, IEquatable<CurriculumVitae>
    {
        public bool TornarPublico { get; set; }
        public Cidadao Cidadao { get; set; }
        public Funcao Funcao { get; set; }
        public SetorMercado SetorMercado { get; set; }
        public ISet<CVCompetencia> CVCompetenciaSet { get; set; } = new HashSet<CVCompetencia>();
        public ISet<CVHabilidade> CVHabilidadeSet { get; set; } = new HashSet<CVHabilidade>();
        public ISet<CVExperiencia> CVExperienciaSet { get; set; } = new HashSet<CVExperiencia>();
        public ISet<VagaCV> VagaCVSet { get; set; } = new HashSet<VagaCV>();

        public bool Equals(CurriculumVitae other)
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