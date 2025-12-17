using System;
using System.Collections.Generic;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Saude
{
    public class ServicoSaude : BaseEntity<int>, IEquatable<ServicoSaude>
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }

        public ISet<UbsServicoSaude> UbsServicoSaudeSet { get; set; }

        public bool Equals(ServicoSaude other)
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