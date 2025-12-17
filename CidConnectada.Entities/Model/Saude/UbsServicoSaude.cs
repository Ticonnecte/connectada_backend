using System;
using AutoMapper;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Saude
{
    public class UbsServicoSaude : BaseEntity<UbsServicoSaudeKey>, IEquatable<UbsServicoSaude>
    {
        [IgnoreMap]
        public override UbsServicoSaudeKey Key => new UbsServicoSaudeKey
        {
            UbsId = UbsId,
            ServicoSaudeId = ServicoSaudeId
        };

        public string UbsId { get; set; }
        public int ServicoSaudeId { get; set; }
        public UnidadeBasicaSaude UnidadeBasicaSaude { get; set; }
        public ServicoSaude ServicoSaude { get; set; }

        public bool Equals(UbsServicoSaude other)
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