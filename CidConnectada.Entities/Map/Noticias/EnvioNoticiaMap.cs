using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Noticias;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Noticias
{
    public class EnvioNoticiaMap : EntityBaseMap<EnvioNoticia, EnvioNoticiaKey>
    {
        public EnvioNoticiaMap()
        {
            ToTable("ENVIO_NOTICIA");

            Property(e => e.NoticiaId)
                .HasColumnName("NOTICIA_ID").IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.UsuarioId)
                .HasColumnName("USUARIO_ID").IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.DhEnvio)
                .HasColumnName("DH_ENVIO");

            Property(e => e.DeliveryEvent)
                .HasColumnName("DELIVERY_EVENT");

            Property(e => e.ReceivedEvent)
                .HasColumnName("RECEIVED_EVENT");

            Property(e => e.ReadEvent)
                .HasColumnName("READ_EVENT");

            Property(e => e.StatusEnum)
                .HasColumnName("STATUS_ENUM");

            Property(e => e.ZaapId)
                .HasColumnName("ZAAP_ID")
                .HasMaxLength(100);

            Property(e => e.MessageId)
                .HasColumnName("MESSAGE_ID")
                .HasMaxLength(100);

            HasRequired(e => e.Noticia).WithMany(e => e.EnvioNoticiaSet).HasForeignKey(e => e.NoticiaId);
            HasRequired(e => e.Usuario).WithMany(e => e.EnvioNoticiaSet).HasForeignKey(e => e.UsuarioId);
        }

        protected override void DefineHasKey()
        {
            HasKey(entity => new
            {
                entity.NoticiaId,
                entity.UsuarioId
            });
        }
    }
}