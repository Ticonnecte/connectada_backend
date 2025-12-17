using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Emprego;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Emprego
{
    public class CVExperienciaMap : EntityBaseMap<CVExperiencia, CVExperienciaKey>
    {
        public CVExperienciaMap()
        {
            ToTable("CV_EXPERIENCIA", "emprego");

            Property(e => e.CVId)
                .HasColumnName("CV_ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.ItemIndex)
                .HasColumnName("ITEM_INDEX")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.NomeEmpresa)
                .HasColumnName("NOME_EMPRESA")
                .HasMaxLength(50);

            Property(e => e.PeriodoInicio)
                .HasColumnName("PERIODO_INICIO");

            Property(e => e.PeriodoFinal)
                .HasColumnName("PERIODO_FINAL");

            Property(e => e.Atividades)
                .HasColumnName("ATIVIDADES")
                .HasMaxLength(4000);

            HasRequired(e => e.CurriculumVitae).WithMany(e => e.CVExperienciaSet).HasForeignKey(e => e.CVId);
            HasRequired(e => e.Funcao).WithMany(e => e.CVExperienciaSet).Map(e => e.MapKey("FUNCAO_ID"));
        }

        protected override void DefineHasKey()
        {
            HasKey(entity => new
            {
                entity.CVId,
                entity.ItemIndex
            });
        }
    }
}