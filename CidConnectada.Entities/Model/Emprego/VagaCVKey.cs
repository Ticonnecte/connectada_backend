using System;
using Zenite.Pi.Entities;

namespace CidConnectada.Entities.Model.Emprego
{
    public class VagaCVKey : IEntityKey
    {
        public long OfertaVagaId { get; set; }
        public int CVId { get; set; }

        public int CompareTo(object obj)
        {
            int result;
            if (obj is VagaCVKey)
            {
                result = OfertaVagaId.CompareTo(((VagaCVKey)obj).OfertaVagaId);
                if (result == 0)
                    result = CVId.CompareTo(((VagaCVKey)obj).CVId);
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
                OfertaVagaId, CVId
            };
        }
    }
}