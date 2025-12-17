using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Emprego;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Emprego
{
    public class CVCompetenciaMap : EntityBaseMap<CVCompetencia, CVCompetenciaKey>
    {
        public CVCompetenciaMap()
        {
            ToTable("CV_COMPETENCIA", "emprego");

            Property(e => e.CVId)
                .HasColumnName("CV_ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.CompetenciaId)
                .HasColumnName("COMPETENCIA_ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            HasRequired(e => e.CurriculumVitae).WithMany(e => e.CVCompetenciaSet).HasForeignKey(e => e.CVId);
            HasRequired(e => e.Competencia).WithMany(e => e.CVCompetenciaSet).HasForeignKey(e => e.CompetenciaId);
        }

        protected override void DefineHasKey()
        {
            HasKey(entity => new
            {
                entity.CVId,
                entity.CompetenciaId
            });
        }
    }
}