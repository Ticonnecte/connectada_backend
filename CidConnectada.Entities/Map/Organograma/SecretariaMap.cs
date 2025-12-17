using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Organograma;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Organograma
{
    public class SecretariaMap : EntityBaseMap<Secretaria, string>
    {
        public SecretariaMap()
        {
            ToTable("SECRETARIA", "organo");

            Property(e => e.Key)
                .IsRequired()
                .HasColumnName("ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("NOME");

            Property(e => e.NomeSecretario)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("NOME_SECRETARIO");

            Property(e => e.IconeNome)
                .HasColumnName("ICONE_NOME")
                .HasMaxLength(50)
                .IsRequired();

            Property(e => e.OrdemHome)
                .HasColumnName("ORDEM_HOME");

            Property(e => e.IsActive)
                .HasColumnName("IS_ACTIVE");

            Property(e => e.TenantKey)
                .HasColumnName("TENANT_ID")
                .IsRequired();

            HasRequired(e => e.Prefeitura).WithMany(e => e.SecretariaSet).HasForeignKey(e => e.TenantKey);

        }
    }
}