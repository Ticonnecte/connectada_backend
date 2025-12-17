using System;
using Zenite.Pi.Entities;

namespace CidConnectada.Entities.Model.Saude
{
    public class UbsServicoSaudeKey : IEntityKey
    {
        public string UbsId { get; set; }
        public int ServicoSaudeId { get; set; }

        public int CompareTo(object obj)
        {
            int result;
            if (obj is UbsServicoSaudeKey)
            {
                result = UbsId.CompareTo(((UbsServicoSaudeKey)obj).UbsId);
                if (result == 0)
                    result = ServicoSaudeId.CompareTo(((UbsServicoSaudeKey)obj).ServicoSaudeId);
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
                UbsId, ServicoSaudeId
            };
        }
    }
}