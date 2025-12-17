using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Saude;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Saude
{
    public class UbsEspecialidadeMedicaMap : EntityBaseMap<UbsEspecialidadeMedica, UbsEspecialidadeMedicaKey>
    {
        public UbsEspecialidadeMedicaMap()
        {
            ToTable("UBS_ESPECIALIDADE_MEDICA", "saude");

            Property(e => e.UbsId)
                .HasColumnName("UBS_ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.EspecMedId)
                .HasColumnName("ESPEC_MED_ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            HasRequired(e => e.UnidadeBasicaSaude)
                .WithMany(e => e.UbsEspecialidadeMedicaSet)
                .HasForeignKey(e => e.UbsId);

            HasRequired(e => e.EspecialidadeMedica)
                .WithMany(e => e.UbsEspecialidadeMedicaSet)
                .HasForeignKey(e => e.EspecMedId);
        }

        protected override void DefineHasKey()
        {
            HasKey(entity => new
            {
                entity.UbsId,
                entity.EspecMedId
            });
        }
    }
}