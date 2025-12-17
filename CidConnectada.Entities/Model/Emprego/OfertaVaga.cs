using System;
using System.Collections.Generic;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Entities.Model.Local;
using CidConnectada.Entities.Model.Organograma;
using Zenite.Pi.Entities.Model.MultiTenancy;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Emprego
{
    public class OfertaVaga : MultiTenancy<long, int>, IEquatable<OfertaVaga>
    {
        public string NomeEmpresa { get; set; }
        public byte ExperienciaMin { get; set; }
        public TimeSpan? HorarioInicio { get; set; }
        public TimeSpan? HorarioFinal { get; set; }
        public DateTime DhCriacao { get; set; }
        public OfertaVagaStatusEnum StatusEnum { get; set; }

        public virtual Cidadao Empregador { get; set; }
        public Funcao Funcao { get; set; }
        public Endereco Endereco { get; set; }
        public FaixaSalarial FaixaSalarial { get; set; }
        public SetorMercado SetorMercado { get; set; }
        public Prefeitura Prefeitura { get; set; }
        public ISet<OfertaVagaCompetencia> OfertaVagaCompetenciaSet { get; set; } = new HashSet<OfertaVagaCompetencia>();
        public ISet<OfertaVagaHabilidade> OfertaVagaHabilidadeSet { get; set; } = new HashSet<OfertaVagaHabilidade>();
        public ISet<VagaCV> VagaCVSet { get; set; } = new HashSet<VagaCV>();

        public bool Equals(OfertaVaga other)
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