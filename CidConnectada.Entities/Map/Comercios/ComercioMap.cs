using CidConnectada.Entities.Model.Comercios;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Comercios
{
    public class ComercioMap: EntityBaseMap<Comercio, string>
    {
        public ComercioMap()
        {
            ToTable("COMERCIO", "comercio");
            Property(e => e.Key)
                .HasColumnName("ID")
                .IsRequired();

            Property(e => e.Nome)
                .HasColumnName("NOME")
                .HasMaxLength(50)
                .IsRequired();
            
            Property(e => e.Descricao)
                .HasColumnName("DESCRICAO")
                .HasMaxLength(400);
            
            Property(e => e.NumeroWhatsApp)
                .HasColumnName("NUMERO_WHATASAPP")
                .HasMaxLength(50)
                .IsRequired();
            
            Property(e => e.OrdemHome)
                .HasColumnName("ORDEM_HOME");
            
            Property(e => e.ImgUrl)
                .HasMaxLength(255)
                .HasColumnName("IMG_URL");

            Property(e => e.ImgHashCode)
                .HasColumnName("IMG_HASH_CODE");
            
            Property(e => e.IsActive)
                .HasColumnName("IS_ACTIVE");
            
            Property(e => e.AbreAs)
                .HasColumnName("ABRE_AS")
                .IsRequired();
            
            Property(e => e.FechaAs)
                .HasColumnName("FECHA_AS")
                .IsRequired();
            
            Property(e => e.TenantKey)
                .HasColumnName("TENANT_ID")
                .IsRequired();
            
            HasRequired(e => e.TipoComercio).WithMany(e => e.ComercioSet).Map(m => m.MapKey("TIPO_COMERCIO_ID"));
            HasRequired(e => e.Cidadao).WithMany(e => e.ComercioSet).Map(m => m.MapKey("CIDADAO_ID"));
            HasRequired(e => e.Endereco).WithMany(e => e.ComercioSet).Map(m => m.MapKey("ENDERECO_ID"));
        }
    }
}
