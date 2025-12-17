using System;
using System.Collections.Generic;
using AutoMapper;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Comunicacao
{
    public class EnqueteOpcao : BaseEntity<EnqueteOpcaoKey>, IEquatable<EnqueteOpcao>
    {
        [IgnoreMap]
        public override EnqueteOpcaoKey Key
        {
            get => new EnqueteOpcaoKey
            {
                EnqueteId = EnqueteId,
                OpcaoIdx = OpcaoIdx
            };
        }

        public int EnqueteId { get; set; }
        public byte OpcaoIdx { get; set; }
        public string Texto { get; set; }

        public Enquete Enquete { get; set; }
        public ISet<EnqueteResposta> EnqueteRespostaSet { get; set; } = new HashSet<EnqueteResposta>();

        public bool Equals(EnqueteOpcao other)
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