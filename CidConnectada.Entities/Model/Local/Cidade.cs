using System;
using System.Collections.Generic;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Local
{
    public class Cidade : BaseEntity<int>, IEquatable<Cidade>
    {
        public string Nome { get; set; }
        public Estado Estado { get; set; }
        public ISet<Bairro> BairroSet { get; set; } = new HashSet<Bairro>();
        public ISet<Endereco> EnderecoSet { get; set; } = new HashSet<Endereco>();
        public bool Equals(Cidade other)
        {
            bool result;
            if (ReferenceEquals(other, null))
            {
                result = false;
            }
            else if (ReferenceEquals(other, this))
            {
                result = true;
            }
            else
            {
                result = EntityUtil.EqualsEntity(this, other);
            }
            return result;
        }
    }
}