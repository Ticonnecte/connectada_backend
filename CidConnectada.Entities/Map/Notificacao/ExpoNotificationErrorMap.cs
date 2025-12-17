using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Notificacao;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Notificacao
{
    public class ExpoNotificationErrorMap : EntityBaseMap<ExpoNotificationError, ExpoNotificationErrorKey>
    {
        public ExpoNotificationErrorMap()
        {
            ToTable("EXPO_NOTIFICATION_ERROR", "msg");

            Property(e => e.ExpoNotificationTokenId)
                .HasColumnName("EXPO_NOTIFICATION_TOKEN_ID")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.Code)
                .HasColumnName("CODE")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.Message)
                .HasColumnName("MESSAGE")
                .HasMaxLength(1000);

            HasRequired(e => e.ExpoNotificationToken).WithMany(e => e.ExpoNotificationErrorSet).HasForeignKey(e => e.ExpoNotificationTokenId);
        }

        protected override void DefineHasKey()
        {
            HasKey(entity => new { entity.ExpoNotificationTokenId, entity.Code });
        }
    }
}