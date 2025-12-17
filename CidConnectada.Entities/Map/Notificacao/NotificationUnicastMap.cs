using CidConnectada.Entities.Model.Notificacao;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Notificacao
{
    public class NotificationUnicastMap : EntityBaseMap<NotificationUnicast, int>
    {
        public NotificationUnicastMap()
        {
            ToTable("NOTIFICATION_UNICAST", "msg");

            HasRequired(e => e.Usuario).WithMany(e => e.NotificationUnicastSet).Map(e => e.MapKey("USER_ID"));
        }
    }
}