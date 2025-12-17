using System;
using Zenite.Pi.Entities;

namespace CidConnectada.Entities.Model.Notificacao
{
    public class NotificationUserKey : IEntityKey
    {
        public int UsuarioId { get; set; }
        public int NotificationId { get; set; }

        public int CompareTo(object obj)
        {
            int result;
            if (obj is NotificationUserKey)
            {
                result = UsuarioId.CompareTo(((NotificationUserKey)obj).UsuarioId);
                if (result == 0)
                {
                    result = NotificationId.CompareTo(((NotificationUserKey)obj).NotificationId);
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
            return new object[2] { NotificationId, UsuarioId };
        }
    }
}