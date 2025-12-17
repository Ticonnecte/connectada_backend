using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Emprego;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Emprego
{
    public class SetorMercadoMap : EntityBaseMap<SetorMercado, int>
    {
        public SetorMercadoMap()
        {
            ToTable("SETOR_DO_MERCADO", "emprego");

            Property(e => e.Key)
                .HasColumnName("ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Nome)
                .HasColumnName("NOME")
                .HasMaxLength(50);
        }
    }
}