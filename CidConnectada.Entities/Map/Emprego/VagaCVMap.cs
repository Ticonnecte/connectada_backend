using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Emprego;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Emprego
{
    public class VagaCVMap : EntityBaseMap<VagaCV, VagaCVKey>
    {
        public VagaCVMap()
        {
            ToTable("VAGA_CV", "emprego");

            Property(e => e.OfertaVagaId)
                .HasColumnName("OFERTA_DE_VAGA_ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.CVId)
                .HasColumnName("CV_ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.GrauCorrelacao)
                .HasColumnName("GRAU_CORRELACAO")
                .HasPrecision(5, 2);

            Property(e => e.InteresseCandidato)
                .HasColumnName("INTERESSE_CANDIDATO");

            Property(e => e.InteresseEmpregador)
                .HasColumnName("INTERESSE_EMPREGADOR");

            HasRequired(e => e.OfertaVaga).WithMany(e => e.VagaCVSet).HasForeignKey(e => e.OfertaVagaId);
            HasRequired(e => e.CurriculumVitae).WithMany(e => e.VagaCVSet).HasForeignKey(e => e.CVId);
        }

        protected override void DefineHasKey()
        {
            HasKey(entity => new
            {
                entity.OfertaVagaId,
                entity.CVId
            });
        }
    }
}