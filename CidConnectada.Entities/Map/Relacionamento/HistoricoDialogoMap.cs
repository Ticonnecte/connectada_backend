using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Relacionamento;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Relacionamento
{
    public class HistoricoDialogoMap : EntityBaseMap<HistoricoDialogo, HistoricoDialogoKey>
    {
        public HistoricoDialogoMap()
        {

            ToTable("HISTORICO_DIALOGO", "relac");

            Property(e => e.DialogoId)
                .IsRequired()
                .HasColumnName("DIALOGO_ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.SequenciaIndex)
                .IsRequired()
                .HasColumnName("SEQUENCIA_INDEX")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.Descricao)
                .HasColumnName("DESCRICAO")
                .HasMaxLength(4000);

            Property(e => e.DhTransicao)
                .IsRequired()
                .HasColumnName("DH_TRANSICAO");

            Property(e => e.DhTransicaoStr)
                .HasColumnName("DH_TRANSICAO_STR")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            Property(e => e.StatusEnum)
                .HasColumnName("STATUS_ENUM")
                .IsRequired();

            HasRequired(e => e.Dialogo).WithMany(e => e.HistoricoDialogoSet).HasForeignKey(e => e.DialogoId);
            HasRequired(e => e.Funcionario).WithMany(e => e.HistoricoDialogoSet).Map(e => e.MapKey("FUNCIONARIO_ID"));
        }
        protected override void DefineHasKey()
        {
            HasKey(entity => new
            {
                entity.DialogoId,
                entity.SequenciaIndex
            });
        }
    }
}