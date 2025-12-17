using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Comunicacao;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Comunicacao
{
    public class EnqueteRespostaMap : EntityBaseMap<EnqueteResposta, int>
    {
        public EnqueteRespostaMap()
        {
            ToTable("ENQUETE_RESPOSTA", "comunicacao");

            Property(e => e.Key)
                .HasColumnName("ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(e => e.EnqueteOpcao).WithMany(e => e.EnqueteRespostaSet).Map(e => e.MapKey("ENQUETE_ID", "OPCAO_IDX"));
            HasRequired(e => e.Usuario).WithMany(e => e.EnqueteRespostaSet).Map(e => e.MapKey("USUARIO_ID"));
        }
    }
}