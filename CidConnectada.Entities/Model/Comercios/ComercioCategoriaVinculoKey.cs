using System;
using CidConnectada.Entities.Model.Notificacao;
using Zenite.Pi.Entities;

namespace CidConnectada.Entities.Model.Comercios
{
    public class ComercioCategoriaVinculoKey: IEntityKey
    {
        public string ComericoId { get; set; }
        public int CategoriaId { get; set; }

        public int CompareTo(object obj)
        {
            int result;
            if (obj is NotificationUserKey)
            {
                result = ComericoId.CompareTo(((ComercioCategoriaVinculoKey)obj).ComericoId);
                if (result == 0)
                {
                    result = CategoriaId.CompareTo(((ComercioCategoriaVinculoKey)obj).CategoriaId);
                }
            }
            else
            {
                throw new TypeInitializationException(obj.GetType().FullName, null);
            }
            return result;
        }

        public object[] ToArray()
        {
            return new object[2] { ComericoId, CategoriaId };
        }

    }
}
