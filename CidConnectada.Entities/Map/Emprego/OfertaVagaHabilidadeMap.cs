using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Emprego;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Emprego
{
    public class OfertaVagaHabilidadeMap : EntityBaseMap<OfertaVagaHabilidade, OfertaVagaHabilidadeKey>
    {
        public OfertaVagaHabilidadeMap()
        {
            ToTable("OFERTA_DE_VAGA_HABILIDADE", "emprego");

            Property(e => e.OfertaVagaId)
                .HasColumnName("OFERTA_DE_VAGA_ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.HabilidadeId)
                .HasColumnName("HABILIDADE_ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            HasRequired(e => e.OfertaVaga).WithMany(e => e.OfertaVagaHabilidadeSet).HasForeignKey(e => e.OfertaVagaId);
            HasRequired(e => e.Habilidade).WithMany(e => e.OfertaVagaHabilidadeSet).HasForeignKey(e => e.HabilidadeId);
        }

        protected override void DefineHasKey()
        {
            HasKey(entity => new
            {
                entity.OfertaVagaId,
                entity.HabilidadeId
            });
        }
    }
}