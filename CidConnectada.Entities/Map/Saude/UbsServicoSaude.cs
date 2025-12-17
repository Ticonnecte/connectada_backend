using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Saude;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Saude
{
    public class UbsServicoSaudeMap : EntityBaseMap<UbsServicoSaude, UbsServicoSaudeKey>
    {
        public UbsServicoSaudeMap()
        {
            ToTable("UBS_SERVICO_SAUDE", "saude");

            Property(e => e.UbsId)
                .HasColumnName("UBS_ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.ServicoSaudeId)
                .HasColumnName("SERVICO_SAUDE_ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            HasRequired(e => e.UnidadeBasicaSaude)
                .WithMany(e => e.UbsServicoSaudeSet)
                .HasForeignKey(e => e.UbsId);

            HasRequired(e => e.ServicoSaude)
                .WithMany(e => e.UbsServicoSaudeSet)
                .HasForeignKey(e => e.ServicoSaudeId);
        }

        protected override void DefineHasKey()
        {
            HasKey(entity => new
            {
                entity.UbsId,
                entity.ServicoSaudeId
            });
        }
    }
}