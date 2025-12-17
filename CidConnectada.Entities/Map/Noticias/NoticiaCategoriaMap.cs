using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Noticias;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Noticias
{
    public class NoticiaCategoriaMap : EntityBaseMap<NoticiaCategoria, int>
    {
        public NoticiaCategoriaMap()
        {
            ToTable("NOTICIA_CATEGORIA");

            Property(e => e.Key)
                .HasColumnName("ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Nome)
                .HasColumnName("NOME")
                .HasMaxLength(50);

            Property(e => e.Cor)
                .HasColumnName("COR");

            Property(e => e.Descricao)
                .HasColumnName("DESCRICAO")
                .HasMaxLength(400);

            Property(e => e.IconeNome)
                .HasColumnName("ICONE_NOME")
                .HasMaxLength(255);

            Property(e => e.TenantKey)
                .HasColumnName("TENANT_ID");
            

            HasRequired(e => e.Prefeitura).WithMany().HasForeignKey(e => e.TenantKey);
        }
    }
}