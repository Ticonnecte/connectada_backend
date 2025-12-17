using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Notificacao;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Notificacao
{
    public class ExpoNotificationStatusMap : EntityBaseMap<ExpoNotificationStatus, int>
    {
        public ExpoNotificationStatusMap()
        {
            ToTable("EXPO_NOTIFICATION_STATUS", "msg");

            Property(e => e.Key)
                .HasColumnName("ID")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.SentAt)
                .HasColumnName("SENT_AT")
                .IsRequired();

            Property(e => e.ExpoId)
                .HasColumnName("EXPO_ID")
                .HasMaxLength(255)
                .IsRequired();

            Property(e => e.StatusEnum)
                .HasColumnName("STATUS_ENUM")
                .IsRequired();

            HasRequired(e => e.Notification).WithMany(e => e.ExpoNotificationStatusSet).Map(e => e.MapKey("NOTIFICATION_ID"));
            HasRequired(e => e.ExpoNotificationToken).WithMany(e => e.ExpoNotificationStatusSet).Map(e => e.MapKey("EXPO_NOTIFICATION_TOKEN_ID"));
        }
    }
}