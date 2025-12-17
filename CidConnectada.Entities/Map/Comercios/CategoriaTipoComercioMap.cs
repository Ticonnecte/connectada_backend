using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Comercios;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Comercios
{
    public class CategoriaTipoComercioMap: EntityBaseMap<CategoriaTipoComercio, int>
    {
        public CategoriaTipoComercioMap()
        {
            ToTable("CATEGORIA_TIPO_COMERCIO", "comercio");
            Property(e => e.Key)
                .HasColumnName("ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            Property(e => e.Nome)
                .HasColumnName("NOME")
                .HasMaxLength(50)
                .IsRequired();

            HasRequired(e => e.TipoComercio).WithMany(e => e.CategoriaTipoComercioSet).Map(m => m.MapKey("TPO_COMERCIO_ID"));
        }
    }
}
