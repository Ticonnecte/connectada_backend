using System;
using Zenite.Pi.Entities;

namespace CidConnectada.Entities.Model.Organograma
{
    public class SecretariaMenuKey : IEntityKey
    {
        public string SecretariaId { get; set; }
        public Byte OrdemIdx { get; set; }

        public override bool Equals(object obj)
        {
            return obj is SecretariaMenuKey && GetHashCode() == obj.GetHashCode();
        }

        public override string ToString()
        {
            return SecretariaId.ToString() + OrdemIdx.ToString().PadLeft(11, '0');
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public int CompareTo(object obj)
        {
            int result;
            if (obj is SecretariaMenuKey)
            {
                result = SecretariaId.CompareTo(((SecretariaMenuKey)obj).SecretariaId);
                if (result == 0)
                {
                    result = OrdemIdx.CompareTo(((SecretariaMenuKey)obj).OrdemIdx);
                }
            }
            else
            {
                throw new TypeInitializationException(obj.GetType().FullName, null);
            }
            return result;
        }

        public object[] ToArray()
        {
            return new object[2] { SecretariaId, OrdemIdx };
        }
    }
}