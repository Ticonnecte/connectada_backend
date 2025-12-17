using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Noticias;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Noticias
{
    public class NoticiaMap : EntityBaseMap<Noticia, string>
    {
        public NoticiaMap()
        {
            ToTable("NOTICIA");

            Property(e => e.Key)
                .HasColumnName("ID")
                .HasMaxLength(128)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.Lead)
                .HasColumnName("LEAD")
                .HasMaxLength(400);

            Property(e => e.Ativa)
                .HasColumnName("ATIVA");

            Property(e => e.Conteudo)
                .HasColumnName("CONTEUDO");

            Property(e => e.EnviarWhatsApp)
                .HasColumnName("ENVIAR_WA");

            Property(e => e.FotoCapaUrl)
                .HasColumnName("FOTO_CAPA_URL");

            Property(e => e.ImgHashCode)
                .HasColumnName("IMG_HASH_CODE");

            Property(e => e.TenantKey)
                .HasColumnName("TENANT_ID");

            HasRequired(e => e.Prefeitura).WithMany().HasForeignKey(e => e.TenantKey);
        }
    }
}