using System;
using Zenite.Pi.Entities;

namespace CidConnectada.Entities.Model.Emprego
{
    public class CVExperienciaKey : IEntityKey
    {
        public int CVId { get; set; }
        public byte ItemIndex { get; set; }

        public int CompareTo(object obj)
        {
            int result;
            if (obj is CVExperienciaKey)
            {
                result = CVId.CompareTo(((CVExperienciaKey)obj).CVId);
                if (result == 0)
                    result = ItemIndex.CompareTo(((CVExperienciaKey)obj).ItemIndex);
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
                CVId, ItemIndex
            };
        }
    }
}