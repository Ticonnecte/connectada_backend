using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Local;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Local
{
    public class BairroMap : EntityBaseMap<Bairro, int>
    {
        public BairroMap()
        {
            ToTable("BAIRRO", "local");

            Property(e => e.Key)
                .HasColumnName("ID")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Nome)
                .HasColumnName("NOME")
                .HasMaxLength(100);

            HasRequired(e => e.Cidade).WithMany(e => e.BairroSet).Map(e => e.MapKey("CIDADE_ID"));
        }
    }
}