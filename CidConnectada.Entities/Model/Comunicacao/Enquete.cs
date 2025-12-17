using System;
using System.Collections.Generic;
using Zenite.Pi.Entities.Model.MultiTenancy;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Comunicacao
{
    public class Enquete : MultiTenancy<int, int>, IEquatable<Enquete>
    {
        public string Nome { get; set; }
        public DateTime VigenciaInicio { get; set; }
        public DateTime VigenciaFinal { get; set; }
        public bool IsMultiVal { get; set; }
        public int MetaRespostas { get; set; }
        public string Pergunta { get; set; }

        public ISet<EnqueteOpcao> EnqueteOpcaoSet { get; set; } = new HashSet<EnqueteOpcao>();
        
        public bool Equals(Enquete other)
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