using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Local;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Local
{
    public class CityMap : EntityBaseMap<Cidade, int>
    {
        public CityMap()
        {
            ToTable("CIDADE", "local");

            Property(e => e.Key)
                .HasColumnName("ID")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Nome)
                .HasColumnName("NOME")
                .HasMaxLength(100);

            HasRequired(e => e.Estado).WithMany(e => e.CidadeSet).Map(e => e.MapKey("ESTADO_ID"));
        }
    }
}