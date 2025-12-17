using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Saude;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Saude
{
    public class UnidadeBasicaSaudeMap : EntityBaseMap<UnidadeBasicaSaude, string>
    {
        public UnidadeBasicaSaudeMap()
        {
            ToTable("UNIDADE_BASICA_SAUDE", "saude");

            Property(e => e.ImgHashCode)
                .HasColumnName("IMG_HASHCODE");


            Property(e => e.Key)
                .HasColumnName("ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.Nome)
                .HasColumnName("NOME")
                .HasMaxLength(150);

            Property(e => e.CodigoCNES)
                .HasColumnName("CODIGO_CNES")
                .HasMaxLength(50);

            Property(e => e.TipoUnidadeEnum)
                .HasColumnName("TIPO_UNIDADE_ENUM");

            Property(e => e.PorteEnum)
                .HasColumnName("PORTE_ENUM");

            Property(e => e.RegiaoAbrangenciaEnum)
                .HasColumnName("REGIAO_ABRANGENCIA_ENUM");

            Property(e => e.AreaTotal)
                .HasColumnName("AREA_TOTAL");

            Property(e => e.NumeroSalas)
                .HasColumnName("NUMERO_SALAS");

            Property(e => e.Acessibilidade)
                .HasColumnName("ACESSIBILIDADE");

            Property(e => e.NumEquipeSaudeFamilia)
                .HasColumnName("NUM_EQUIPE_SAUDE_FAMILIA");

            Property(e => e.NumProfissionais)
                .HasColumnName("NUM_PROFISSIONAIS");

            Property(e => e.HorarioFuncionamentoInicio)
                .HasColumnName("HORARIO_FUNCIONAMENTO_INICIO");

            Property(e => e.HorarioFuncionamentoFinal)
                .HasColumnName("HORARIO_FUNCIONAMENTO_FINAL");

            Property(e => e.CapacidadeAtendimentoDia)
                .HasColumnName("CAPACIDADE_ATENDIMENTO_DIA");

            Property(e => e.ResponsavelNome)
                .HasColumnName("RESPONSAVEL_NOME")
                .HasMaxLength(100);

            Property(e => e.ResponsavelWhatsApp)
                .HasColumnName("RESPONSAVEL_WHATSAPP")
                .HasMaxLength(15);

            Property(e => e.VinculacaoAdmnistrativa)
                .HasColumnName("VINCULACAO_ADMINISTRATIVA")
                .HasMaxLength(50);

            Property(e => e.SituacaoEnum)
                .HasColumnName("SITUACAO_ENUM");

            Property(e => e.ImagemUrl)
                .HasColumnName("IMAGEM_URL");

            Property(e => e.TenantKey)
                .HasColumnName("TENANT_ID");

            HasOptional(e => e.Endereco).WithMany(e => e.UBSSet).Map(m => m.MapKey("ENDERECO_ID"));

            HasRequired(e => e.Prefeitura)
                .WithMany(e => e.UnidadeBasicaSaudeSet)
                .HasForeignKey(e => e.TenantKey)
                .WillCascadeOnDelete(false);
        }
    }
}
