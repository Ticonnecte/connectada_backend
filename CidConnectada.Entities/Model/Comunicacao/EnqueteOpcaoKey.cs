using System;
using CidConnectada.Entities.Model.Emprego;
using Zenite.Pi.Entities;

namespace CidConnectada.Entities.Model.Comunicacao
{
    public class EnqueteOpcaoKey : IEntityKey
    {
        public int EnqueteId { get; set; }
        public byte OpcaoIdx { get; set; }

        public int CompareTo(object obj)
        {
            int result;
            if (obj is CVCompetenciaKey)
            {
                result = EnqueteId.CompareTo(((CVCompetenciaKey)obj).CVId);
                if (result == 0)
                    result = OpcaoIdx.CompareTo(((CVCompetenciaKey)obj).CompetenciaId);
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
                EnqueteId, OpcaoIdx
            };
        }
    }
}