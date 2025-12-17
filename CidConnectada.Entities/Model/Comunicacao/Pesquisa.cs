using System;
using Zenite.Pi.Entities.Model.MultiTenancy;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Comunicacao
{
    public class Pesquisa : MultiTenancy<int, int>, IEquatable<Pesquisa>
    {
        public string Nome { get; set; }
        public DateTime VigenciaInicio { get; set; }
        public DateTime VigenciaFinal { get; set; }
        public string GoogleFormsUrl { get; set; }

        public bool Equals(Pesquisa other)
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