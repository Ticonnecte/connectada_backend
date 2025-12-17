using System;
using Zenite.Pi.Entities;

namespace CidConnectada.Entities.Model.Relacionamento
{
    public class HistoricoDialogoKey : IEntityKey
    {
        public string DialogoId { get; set; }
        public int SequenciaIndex { get; set; }

        public int CompareTo(object obj)
        {
            int result;
            if (obj is HistoricoDialogoKey)
            {
                result = DialogoId.CompareTo(((HistoricoDialogoKey)obj).DialogoId);
                if (result == 0)
                    result = SequenciaIndex.CompareTo(((HistoricoDialogoKey)obj).SequenciaIndex);
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
                DialogoId, SequenciaIndex
            };
        }
    }
}