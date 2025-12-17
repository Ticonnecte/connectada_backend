using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Noticias;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Noticias
{
    public class NoticiaCategoriaVincMap : EntityBaseMap<NoticiaCategoriaVinc, NoticiaCategoriaVincKey>
    {
        public NoticiaCategoriaVincMap()
        {
            ToTable("NOTICIA_CATEGORIA_VINC");

            Property(e => e.NoticiaId)
                .HasColumnName("NOTICIA_ID").IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.CategoriaId)
                .HasColumnName("CATEGORIA_ID").IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            HasRequired(e => e.Noticia).WithMany(e => e.NoticiaCategoriaVincSet).HasForeignKey(e => e.NoticiaId);
            HasRequired(e => e.NoticiaCategoria).WithMany(e => e.NoticiaCategoriaVincSet)
                .HasForeignKey(e => e.CategoriaId);
        }

        protected override void DefineHasKey()
        {
            HasKey(entity => new { entity.NoticiaId, entity.CategoriaId });
        }
    }
}