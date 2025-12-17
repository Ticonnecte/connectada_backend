using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Emprego;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Emprego
{
    public class OfertaVagaMap : EntityBaseMap<OfertaVaga, long>
    {
        public OfertaVagaMap()
        {
            ToTable("OFERTA_DE_VAGA", "emprego");

            Property(e => e.Key)
                .HasColumnName("ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.NomeEmpresa)
                .HasColumnName("NOME_EMPRESA")
                .HasMaxLength(50);

            Property(e => e.ExperienciaMin)
                .HasColumnName("EXPERIENCIA_MIN");

            Property(e => e.HorarioInicio)
                .HasColumnName("HORARIO_INICIO");

            Property(e => e.HorarioFinal)
                .HasColumnName("HORARIO_FINAL");

            Property(e => e.DhCriacao)
                .HasColumnName("DH_CRIACAO");

            Property(e => e.StatusEnum)
                .HasColumnName("STATUS_ENUM");

            Property(e => e.TenantKey)
                .HasColumnName("TENANT_ID");

            HasRequired(e => e.Prefeitura).WithMany().HasForeignKey(e => e.TenantKey);
            HasRequired(e => e.Empregador).WithMany(e => e.OfertaVagaSet).Map(e => e.MapKey("CIDADAO_ID"));
            HasOptional(e => e.FaixaSalarial).WithMany(e => e.OfertaVagaSet).Map(e => e.MapKey("FAIXA_SALARIAL_ID"));
            HasOptional(e => e.Funcao).WithMany(e => e.OfertaVagaSet).Map(e => e.MapKey("FUNCAO_ID"));
            HasOptional(e => e.SetorMercado).WithMany(e => e.OfertaVagaSet).Map(e => e.MapKey("SETOR_ID"));
            HasOptional(e => e.Endereco).WithMany(e => e.OfertaVagaSet).Map(e => e.MapKey("ENDERECO_ID"));
        }
    }
}