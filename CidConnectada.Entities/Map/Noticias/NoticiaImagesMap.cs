using CidConnectada.Entities.Model.Infos;
using CidConnectada.Entities.Model.Noticias;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using Zenite.Pi.Entities.Mapping;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace CidConnectada.Entities.Map.Noticias
{
    public class NoticiaImagesMap : EntityBaseMap<NoticiaImages, HtmlImagesKey>
    {
        public NoticiaImagesMap()
        {
            // Problema pontencial do útil copy and paste.
            // ERRO:
            //`payload`. Value: ApiError: An error occurred while executing the command definition.See the inner exception for details.
            //Invalid object name 'info.NOTICIA_IMAGES'.
            //ToTable("NOTICIA_IMAGES", "info");
            ToTable("NOTICIA_IMAGES");

            Property(e => e.HashId)
                .HasColumnName("HASH_ID")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.ParentId)
                .HasColumnName("NOTICIA_ID")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.ImgUrl)
                .HasMaxLength(1000)
                .IsRequired()
                .HasColumnName("IMG_URL");

            HasRequired(e => e.Noticia).WithMany(e => e.NoticiaImagesSet).HasForeignKey(e => e.ParentId);
        }

        protected override void DefineHasKey()
        {
            HasKey(entity => new
            {
                entity.HashId,
                entity.ParentId
            });
        }

    }
}
