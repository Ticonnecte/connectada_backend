using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Emprego;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Emprego
{
    public class CVHabilidadeMap : EntityBaseMap<CVHabilidade, CVHabilidadeKey>
    {
        public CVHabilidadeMap()
        {
            ToTable("CV_HABILIDADE", "emprego");

            Property(e => e.CVId)
                .HasColumnName("CV_ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.HabilidadeId)
                .HasColumnName("HABILIDADE_ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            HasRequired(e => e.CurriculumVitae).WithMany(e => e.CVHabilidadeSet).HasForeignKey(e => e.CVId);
            HasRequired(e => e.Habilidade).WithMany(e => e.CVHabilidadeSet).HasForeignKey(e => e.HabilidadeId);
        }

        protected override void DefineHasKey()
        {
            HasKey(entity => new
            {
                entity.CVId,
                entity.HabilidadeId
            });
        }
    }
}