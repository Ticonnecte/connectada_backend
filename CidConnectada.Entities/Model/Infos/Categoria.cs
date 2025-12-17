using System;
using System.Collections.Generic;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Entities.Model.Organograma;
using Zenite.Pi.Entities.Model.MultiTenancy;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Infos
{
    public class Categoria : MultiTenancy<int, int>, IEquatable<Categoria>
    {
        public string Nome { get; set; }
        public CorEnum Cor { get; set; }
        public string Descricao { get; set; }
        public string IconeNome { get; set; }

        public bool Ativa {  get; set; }
        public ISet<Info> InfoSet { get; set; } = new HashSet<Info>();

        public bool Equals(Categoria other)
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