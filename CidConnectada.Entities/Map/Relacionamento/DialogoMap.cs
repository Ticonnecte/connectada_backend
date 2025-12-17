using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Relacionamento;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Relacionamento
{
    public class DialogoMap : EntityBaseMap<Dialogo, string>
    {
        public DialogoMap()
        {
            ToTable("DIALOGO", "relac");

            Property(e => e.Key)
                .HasColumnName("ID")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.Titulo)
                .HasColumnName("TITULO")
                .HasMaxLength(50);

            Property(e => e.Descricao)
                .HasColumnName("DESCRICAO")
                .HasMaxLength(4000);

            Property(e => e.DhCriacao)
                .HasColumnName("DH_CRIACAO");

            Property(e => e.DhCriacaoStr)
                .HasColumnName("DH_CRIACAO_STR")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            Property(e => e.AssuntoDialogoEnum)
                .HasColumnName("ASSUNTO_DIALOGO_ENUM");

            Property(e => e.DialogoStatusEnum)
                .HasColumnName("STATUS_ENUM");

            Property(e => e.ImagemUrl)
                .HasColumnName("IMAGEM_URL");

            Property(e => e.ImgHashCode)
                .HasColumnName("IMG_HASH_CODE");

            Property(e => e.DataPrevistaExecuacao)
                .HasColumnName("DATA_PREVISTA_EXECUCAO");

            Property(e => e.DataPrevistaFinalizacao)
                .HasColumnName("DATA_PREVISTA_FINALIZACAO");

            Property(e => e.TenantKey)
                .HasColumnName("TENANT_ID")
                .IsRequired();

            HasRequired(e => e.Secretaria).WithMany(e => e.DialogoSet).Map(e => e.MapKey("SECRETARIA_ID"));
            HasOptional(e => e.Cidadao).WithMany(e => e.DialogoSet).Map(e => e.MapKey("CIDADAO_ID"));
            HasRequired(e => e.Endereco).WithMany(e => e.DialogoSet).Map(e => e.MapKey("ENDERECO_ID"));

        }
    }
}