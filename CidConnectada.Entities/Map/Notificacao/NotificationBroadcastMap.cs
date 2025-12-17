using CidConnectada.Entities.Model.Notificacao;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Notificacao
{
    public class NotificationBroadcastMap : EntityBaseMap<NotificationBroadcast, int>
    {
        public NotificationBroadcastMap()
        {
            ToTable("NOTIFICATION_BROADCAST", "msg");
        }
    }
}