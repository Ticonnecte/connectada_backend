using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Notificacao;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Notificacao
{
    public class NotificationMulticastUserMap : EntityBaseMap<NotificationMulticastUser, NotificationUserKey>
    {
        public NotificationMulticastUserMap()
        {
            ToTable("NOTIFICATION_MULTICAST_USER", "msg");

            Property(e => e.NotificationId)
                .HasColumnName("NOTIFICATION_ID")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.UsuarioId)
                .HasColumnName("USUARIO_ID")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            HasRequired(e => e.NotificationMulticast).WithMany(e => e.NotificationMulticastUserSet).HasForeignKey(e => e.NotificationId);
            HasRequired(e => e.Usuario).WithMany(e => e.NotificationMulticastUserSet).HasForeignKey(e => e.UsuarioId);
        }

        protected override void DefineHasKey()
        {
            HasKey(entity => new { entity.NotificationId, entity.UsuarioId });
        }
    }
}