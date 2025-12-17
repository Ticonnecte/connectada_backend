using CidConnectada.Entities.Model.Notificacao;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Notificacao
{
    public class WaNotificacaoStatusMap: EntityBaseMap<WaNotificacaoStatus, NotificationUserKey>
    {
        public WaNotificacaoStatusMap()
        {
            ToTable("WA_NOTIFICATION_STATUS", "msg");

            Property(e => e.NotificationId)
                .HasColumnName("NOTIFICATION_ID")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.UsuarioId)
                .HasColumnName("USUARIO_ID")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.ZaapId)
                .HasColumnName("ZAAP_ID")
                .HasMaxLength(128);

            Property(e => e.ZaapId)
                .HasColumnName("MESSAGE_ID")
                .HasMaxLength(128);

            Property(e => e.SentAt)
                .HasColumnName("SENT_AT");

            Property(e => e.ReceivedAt)
                .HasColumnName("RECEIVED_AT");

            Property(e => e.ReadAt)
                .HasColumnName("READ_AT");

            HasRequired(e => e.Notification).WithMany(e => e.WaNotificacaoStatusSet).HasForeignKey(e => e.NotificationId);
            HasRequired(e => e.Usuario).WithMany(e => e.WaNotificacaoStatusSet).HasForeignKey(e => e.UsuarioId);
        }

        protected override void DefineHasKey()
        {
            HasKey(entity => new { entity.NotificationId, entity.UsuarioId });
        }
    }
}
