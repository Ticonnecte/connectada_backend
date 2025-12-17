using CidConnectada.Entities.Model.Notificacao;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Notificacao
{
    public class NotificationMulticastMap : EntityBaseMap<NotificationMulticast, int>
    {
        public NotificationMulticastMap()
        {
            ToTable("NOTIFICATION_MULTICAST", "msg");
        }
    }
}