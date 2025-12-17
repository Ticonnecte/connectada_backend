using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Emprego;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Emprego
{
    public class CurriculumVitaeMap : EntityBaseMap<CurriculumVitae, int>
    {
        public CurriculumVitaeMap()
        {
            ToTable("CURRICULUM_VITAE", "emprego");

            Property(e => e.Key)
                .HasColumnName("ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.TornarPublico)
                .HasColumnName("TORNAR_PUBLICO");

            HasRequired(e => e.Cidadao).WithMany(e => e.CurriculumVitaeSet).Map(e => e.MapKey("CIDADAO_ID"));
            HasRequired(e => e.Funcao).WithMany(e => e.CurriculumVitaeSet).Map(e => e.MapKey("FUNCAO_ID"));
            HasRequired(e => e.SetorMercado).WithMany(e => e.CurriculumVitaeSet).Map(e => e.MapKey("SETOR_ID"));
        }
    }
}