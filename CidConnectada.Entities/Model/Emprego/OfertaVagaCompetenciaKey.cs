using System;
using Zenite.Pi.Entities;

namespace CidConnectada.Entities.Model.Emprego
{
    public class OfertaVagaCompetenciaKey : IEntityKey
    {
        public long OfertaVagaId { get; set; }
        public int CompetenciaId { get; set; }

        public int CompareTo(object obj)
        {
            int result;
            if (obj is OfertaVagaCompetenciaKey)
            {
                result = OfertaVagaId.CompareTo(((OfertaVagaCompetenciaKey)obj).OfertaVagaId);
                if (result == 0)
                    result = CompetenciaId.CompareTo(((OfertaVagaCompetenciaKey)obj).CompetenciaId);
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
                OfertaVagaId, CompetenciaId
            };
        }
    }
}