using System;
using Zenite.Pi.Entities;

namespace CidConnectada.Entities.Model.Notificacao
{
    public class ExpoNotificationErrorKey : IEntityKey
    {
        public int ExpoNotificationTokenId { get; set; }
        public string Code { get; set; }

        public int CompareTo(object obj)
        {
            int result;
            if (obj is ExpoNotificationErrorKey)
            {
                result = ExpoNotificationTokenId.CompareTo(((ExpoNotificationErrorKey)obj).ExpoNotificationTokenId);
                if (result == 0)
                {
                    result = Code.CompareTo(((ExpoNotificationErrorKey)obj).Code);
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
            return new object[2] { ExpoNotificationTokenId, Code };
        }
    }
}