using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Comercios;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Comercios
{
    public class TipoComercioMap: EntityBaseMap<TipoComercio, int>
    {
        public TipoComercioMap()
        {
            ToTable("TIPO_COMERCIO", "comercio");

            Property(e => e.Key)
                .HasColumnName("ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            Property(e => e.Nome)
                .HasColumnName("NOME")
                .HasMaxLength(50)
                .IsRequired();
            
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
        }
    }
}
