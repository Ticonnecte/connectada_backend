using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Infos;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Infos
{
    public class CategoriaMap : EntityBaseMap<Categoria, int>
    {
        public CategoriaMap()
        {
            ToTable("CATEGORIA", "info");

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

            Property(e => e.Ativa)
                .HasColumnName("ATIVA");

            Property(e => e.TenantKey)
                .HasColumnName("TENANT_ID");

        }
    }
}