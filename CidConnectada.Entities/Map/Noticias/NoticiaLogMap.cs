using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Noticias;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Noticias
{
    public class NoticiaLogMap : EntityBaseMap<NoticiaLog, NoticiaLogKey>
    {
        public NoticiaLogMap()
        {
            ToTable("NOTICIA_LOG");

            Property(e => e.NoticiaId)
                .HasColumnName("NOTICIA_ID").IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.DhUpdate)
                .HasColumnName("DH_UPD").IsRequired()
                .HasColumnType("smalldatetime")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            HasRequired(e => e.Noticia).WithMany(e => e.NoticiaLogSet).HasForeignKey(e => e.NoticiaId);
            HasRequired(e => e.Usuario).WithMany(e => e.NoticiaLogSet).Map(e => e.MapKey("USUARIO_ID"));
        }

        protected override void DefineHasKey()
        {
            HasKey(entity => new { entity.NoticiaId, entity.DhUpdate });
        }
    }
}