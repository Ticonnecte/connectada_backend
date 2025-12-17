using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Notificacao;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Notificacao
{
    public class ExpoNotificationTokenMap : EntityBaseMap<ExpoNotificationToken, int>
    {
        public ExpoNotificationTokenMap()
        {
            ToTable("EXPO_NOTIFICATION_TOKEN", "msg");

            Property(e => e.Key)
                .HasColumnName("ID")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Token)
                .HasColumnName("TOKEN")
                .HasMaxLength(500)
                .IsRequired();

            Property(e => e.CreatedAt)
                .HasColumnName("CREATED_AT")
                .IsRequired();

            Property(e => e.UpdatedAt)
                .HasColumnName("UPDATED_AT");

            HasRequired(e => e.User).WithMany(e => e.ExpoNotificationTokenSet).Map(e => e.MapKey("USER_ID"));
            HasRequired(e => e.Device).WithMany(e => e.ExpoNotificationTokenSet).Map(e => e.MapKey("DEVICE_ID"));
        }
    }
}