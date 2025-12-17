using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Emprego;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Emprego
{
    public class FaixaSalarialMap : EntityBaseMap<FaixaSalarial, int>
    {
        public FaixaSalarialMap()
        {
            ToTable("FAIXA_SALARIAL", "emprego");

            Property(e => e.Key)
                .HasColumnName("ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.ValorMin)
                .HasColumnName("VALOR_MIN");

            Property(e => e.ValorMax)
                .HasColumnName("VALOR_MAX");
        }
    }
}