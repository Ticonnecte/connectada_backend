using System;
using Zenite.Pi.Entities;

namespace CidConnectada.Entities.Model.Saude
{
    public class UbsEspecialidadeMedicaKey : IEntityKey
    {
        public string UbsId { get; set; }
        public int EspecMedId { get; set; }

        public int CompareTo(object obj)
        {
            int result;
            if (obj is UbsEspecialidadeMedicaKey)
            {
                result = UbsId.CompareTo(((UbsEspecialidadeMedicaKey)obj).UbsId);
                if (result == 0)
                    result = EspecMedId.CompareTo(((UbsEspecialidadeMedicaKey)obj).EspecMedId);
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
                UbsId, EspecMedId
            };
        }
    }
}