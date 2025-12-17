using System;
using Zenite.Pi.Entities;

namespace CidConnectada.Entities.Model.Emprego
{
    public class OfertaVagaHabilidadeKey : IEntityKey
    {
        public long OfertaVagaId { get; set; }
        public int HabilidadeId { get; set; }

        public int CompareTo(object obj)
        {
            int result;
            if (obj is OfertaVagaHabilidadeKey)
            {
                result = OfertaVagaId.CompareTo(((OfertaVagaHabilidadeKey)obj).OfertaVagaId);
                if (result == 0)
                    result = HabilidadeId.CompareTo(((OfertaVagaHabilidadeKey)obj).HabilidadeId);
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
                OfertaVagaId, HabilidadeId
            };
        }
    }
}