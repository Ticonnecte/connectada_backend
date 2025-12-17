using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Organograma;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Organograma
{
    public class SecretariaMenuMap : EntityBaseMap<SecretariaMenu, SecretariaMenuKey>
    {
        public SecretariaMenuMap()
        {
            ToTable("SECRETARIA_MENU",  "organo");

            Property(e => e.SecretariaId)
                .HasColumnName("SECRETARIA_ID")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.OrdemIdx)
                .HasColumnName("ORDEM_IDX")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.IconeNome)
                .HasColumnName("ICONE_NOME")
                .HasMaxLength(50)
                .IsRequired();

            Property(e => e.IsActive)
                .HasColumnName("IS_ACTIVE")
                .IsRequired();
                
            Property(e => e.RotaTipoEnum)
                .HasColumnName("ROTA_TIPO_ENUM")
                .IsRequired();
            
            Property(e => e.Path)
                .HasColumnName("PATH")
                .HasMaxLength(1000);

            Property(e => e.Titulo)
                .HasColumnName("TITULO")
                .HasMaxLength(50)
                .IsRequired();

            HasRequired(e => e.Secretaria).WithMany(e => e.SecretariaMenuSet).HasForeignKey(e => e.SecretariaId);
            HasOptional(e => e.RotaInterna).WithMany(e => e.SecretariaMenuSet).Map(e => e.MapKey("ROTA_INTERNA_ID"));
        }

        protected override void DefineHasKey()
        {
            HasKey(entity => new { entity.SecretariaId, entity.OrdemIdx });
        }
    }
}