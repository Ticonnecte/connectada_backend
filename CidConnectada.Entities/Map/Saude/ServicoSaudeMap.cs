using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Saude;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Saude
{
    public class ServicoSaudeMap : EntityBaseMap<ServicoSaude, int>
    {
        public ServicoSaudeMap()
        {
            ToTable("SERVICO_SAUDE", "saude");

            Property(e => e.Key)
                .HasColumnName("ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Nome)
                .HasColumnName("NOME")
                .HasMaxLength(50);

            Property(e => e.Descricao)
                .HasColumnName("DESCRICAO")
                .HasMaxLength(4000);
        }
    }
}