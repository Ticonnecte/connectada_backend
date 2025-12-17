using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Local;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Local
{
    public class EstadoMap : EntityBaseMap<Estado, int>
    {
        public EstadoMap()
        {
            ToTable("ESTADO", "local");

            Property(e => e.Key)
                .HasColumnName("ID")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Nome)
                .HasColumnName("NOME")
                .HasMaxLength(100);

            Property(e => e.Sigla)
                .HasColumnName("SIGLA")
                .HasMaxLength(5);
        }
    }
}