using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Notificacao;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Notificacao
{
    public class NotificationMap : EntityBaseMap<Notification, int>
    {
        public NotificationMap()
        {
            ToTable("NOTIFICATION", "msg");

            Property(e => e.Key)
                .HasColumnName("ID")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.TenantKey)
                .HasColumnName("TENANT_ID")
                .IsRequired();

            Property(e => e.Title)
                .HasColumnName("TITLE")
                .HasMaxLength(255)
                .IsRequired();

            Property(e => e.SubTitle)
                .HasColumnName("SUB_TITLE")
                .HasMaxLength(255);

            Property(e => e.Body)
                .HasColumnName("BODY")
                .HasMaxLength(1000);

            Property(e => e.DataJson)
                .HasColumnName("DATA_JSON")
                .HasMaxLength(2000);

            Property(e => e.DestinoEnum)
                .HasColumnName("DESTINO_ENUM")
                .IsRequired();

            Property(e => e.PrioridadeEnum)
                .HasColumnName("PRIORIDADE_ENUM");

            Property(e => e.StatusEnum)
                .HasColumnName("STATUS_ENUM");

            Property(e => e.DhAgendamento)
                .HasColumnName("DH_AGENDAMENTO");
        }
    }
}