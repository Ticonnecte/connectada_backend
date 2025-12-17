using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Comunicacao;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Comunicacao
{
    public class EnqueteOpcaoMap : EntityBaseMap<EnqueteOpcao, EnqueteOpcaoKey>
    {
        public EnqueteOpcaoMap()
        {
            ToTable("ENQUETE_OPCAO", "comunicacao");

            Property(e => e.EnqueteId)
                .HasColumnName("ENQUETE_ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.OpcaoIdx)
                .HasColumnName("OPCAO_IDX")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.Texto)
                .HasColumnName("OPCAO_TEXTO")
                .HasMaxLength(50);

            HasRequired(e => e.Enquete).WithMany(e => e.EnqueteOpcaoSet).HasForeignKey(e => e.EnqueteId);
        }

        protected override void DefineHasKey()
        {
            HasKey(entity => new
            {
                entity.EnqueteId,
                entity.OpcaoIdx
            });
        }
    }
}