using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Comunicacao;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Comunicacao
{
    public class EnqueteMap : EntityBaseMap<Enquete, int>
    {
        public EnqueteMap()
        {
            ToTable("ENQUETE", "comunicacao");

            Property(e => e.Key)
                .HasColumnName("ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Nome)
                .HasColumnName("NOME")
                .HasMaxLength(100)
                .IsRequired();

            Property(e => e.VigenciaInicio)
                .HasColumnName("VIGENCIA_INICIO");

            Property(e => e.VigenciaFinal)
                .HasColumnName("VIGENCIA_FINAL");

            Property(e => e.IsMultiVal)
                .HasColumnName("IS_MULTI_VAL");

            Property(e => e.MetaRespostas)
                .HasColumnName("META_RESPOSTAS");

            Property(e => e.TenantKey)
                .HasColumnName("TENANT_ID");

            Property(e => e.Pergunta)
                .HasColumnName("PERGUNTA")
                .HasMaxLength(400)
                .IsRequired();
        }
    }
}