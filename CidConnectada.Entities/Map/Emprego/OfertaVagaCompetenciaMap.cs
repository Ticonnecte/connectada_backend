using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Emprego;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Emprego
{
    public class OfertaVagaCompetenciaMap : EntityBaseMap<OfertaVagaCompetencia, OfertaVagaCompetenciaKey>
    {
        public OfertaVagaCompetenciaMap()
        {
            ToTable("OFERTA_DE_VAGA_COMPETENCIA", "emprego");

            Property(e => e.OfertaVagaId)
                .HasColumnName("OFERTA_DE_VAGA_ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.CompetenciaId)
                .HasColumnName("COMPETENCIA_ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            HasRequired(e => e.OfertaVaga).WithMany(e => e.OfertaVagaCompetenciaSet).HasForeignKey(e => e.OfertaVagaId);
            HasRequired(e => e.Competencia).WithMany(e => e.OfertaVagaCompetenciaSet).HasForeignKey(e => e.CompetenciaId);
        }

        protected override void DefineHasKey()
        {
            HasKey(entity => new
            {
                entity.OfertaVagaId,
                entity.CompetenciaId
            });
        }
    }
}