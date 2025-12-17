using System;
using Zenite.Pi.Entities;

namespace CidConnectada.Entities.Model.Emprego
{
    public class CVHabilidadeKey : IEntityKey
    {
        public int CVId { get; set; }
        public int HabilidadeId { get; set; }

        public int CompareTo(object obj)
        {
            int result;
            if (obj is CVHabilidadeKey)
            {
                result = CVId.CompareTo(((CVHabilidadeKey)obj).CVId);
                if (result == 0)
                    result = HabilidadeId.CompareTo(((CVHabilidadeKey)obj).HabilidadeId);
            }
            else
            {
                throw new TypeInitializationException(obj.GetType().FullName, null);
            }

            return result;
        }

        public object[] ToArray()
        {
            return new object[2]
            {
                CVId, HabilidadeId
            };
        }
    }
}