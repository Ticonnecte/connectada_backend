using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Comercios;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Comercios
{
    public class ComercioCategoriaVinculoMap: EntityBaseMap<ComercioCategoriaVinculo, ComercioCategoriaVinculoKey>
    {
        public ComercioCategoriaVinculoMap()
        {
            ToTable("COMERCIO_CATEGORIA_VINCULO", "comercio");
            Property(e => e.ComericoId)
                .HasColumnName("ID_COMERCIO")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.CategoriaId)
                .HasColumnName("ID_CATEGORIA_TIPO_COMERICO")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            HasRequired(e => e.Comercio).WithMany(e => e.ComercioCategoriaVinculoSet).HasForeignKey(e => e.ComericoId);
            HasRequired(e => e.Categoria).WithMany(e => e.ComercioCategoriaVinculoSet).HasForeignKey(e => e.CategoriaId);
        }

        protected override void DefineHasKey()
        {
            HasKey(entity => new { entity.ComericoId, entity.CategoriaId });
        }
    }
}
