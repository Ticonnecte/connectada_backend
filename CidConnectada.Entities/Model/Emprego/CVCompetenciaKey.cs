using System;
using Zenite.Pi.Entities;

namespace CidConnectada.Entities.Model.Emprego
{
    public class CVCompetenciaKey : IEntityKey
    {
        public int CVId { get; set; }
        public int CompetenciaId { get; set; }

        public int CompareTo(object obj)
        {
            int result;
            if (obj is CVCompetenciaKey)
            {
                result = CVId.CompareTo(((CVCompetenciaKey)obj).CVId);
                if (result == 0)
                    result = CompetenciaId.CompareTo(((CVCompetenciaKey)obj).CompetenciaId);
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
                CVId, CompetenciaId
            };
        }
    }
}