using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Emprego;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Emprego
{
    public class HabilidadeMap : EntityBaseMap<Habilidade, int>
    {
        public HabilidadeMap()
        {
            ToTable("HABILIDADE", "emprego");

            Property(e => e.Key)
                .HasColumnName("ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Nome)
                .HasColumnName("NOME");
        }
    }
}