using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Organograma;
using System.ComponentModel.DataAnnotations.Schema;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Organograma
{
    public class PrefeituraMap : EntityBaseMap<Prefeitura, int>
    {
        public PrefeituraMap()
        {
            ToTable("PREFEITURA");
            Property(e => e.Key)
                .HasColumnName("Id").IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Nome)
                .HasColumnName("NOME")
                .IsRequired()
                .HasMaxLength(100);

            Property(e => e.Dominio)
                .HasColumnName("DOMINIO")
                .IsRequired()
                .HasMaxLength(50);

            Property(e => e.BucketName)
                .HasColumnName("BUCKET_NAME")
                .IsRequired()
                .HasMaxLength(100);

            Property(e => e.S3Region)
                .HasColumnName("S3_REGION")
                .IsRequired()
                .HasMaxLength(25);

            Property(e => e.S3AccessKeyId)
                .HasColumnName("S3_ACCESS_KEY_ID")
                .IsRequired()
                .HasMaxLength(50);

            Property(e => e.S3AccessKeySecret)
                .HasColumnName("S3_ACCESS_KEY_SECRET")
                .IsRequired()
                .HasMaxLength(50);

            Property(e => e.ZApiIdInstancia)
                .HasColumnName("ZAPI_ID_INSTANCIA")
                .IsRequired()
                .HasMaxLength(50);

            Property(e => e.ZApiToken)
                .HasColumnName("ZAPI_TOKEN")
                .IsRequired()
                .HasMaxLength(50);

            Property(e => e.ZApiClientToken)
                .HasColumnName("ZAPI_CLIENT_TOKEN")
                .IsRequired()
                .HasMaxLength(50);

            Property(e => e.GoogleMapsApiKey)
                .HasColumnName("GOOGLE_MAPS_API_KEY")
                .HasMaxLength(50);

            Property(e => e.PrimaryMainColor)
                .HasColumnName("PRIMARY_MAIN_COLOR")
                .HasMaxLength(6);
            
            Property(e => e.PrimaryDarkColor)
                .HasColumnName("PRIMARY_DARK_COLOR")
                .HasMaxLength(6);
            
            Property(e => e.PrimaryLightColor)
                .HasColumnName("PRIMARY_LIGHT_COLOR")
                .HasMaxLength(6);
            
            Property(e => e.SecondaryMainColor)
                .HasColumnName("SECONDARY_MAIN_COLOR")
                .HasMaxLength(6);
            
            Property(e => e.SecondaryDarkColor)
                .HasColumnName("SECONDARY_DARK_COLOR")
                .HasMaxLength(6);
            
            Property(e => e.SecondaryLightColor)
                .HasColumnName("SECONDARY_LIGHT_COLOR")
                .HasMaxLength(6);
            
            Property(e => e.LogoHeaderUrl)
                .HasColumnName("LOGO_HEADER_URL")
                .HasMaxLength(255);
            
            Property(e => e.LogoHorizontalUrl)
                .HasColumnName("LOGO_HORIZONTAL_URL")
                .HasMaxLength(255);
            
            Property(e => e.LogoVerticalUrl)
                .HasColumnName("LOGO_VERTICAL_URL")
                .HasMaxLength(255);
            
            Property(e => e.Facebook)
                .HasColumnName("FACEBOOK")
                .HasMaxLength(255);
            
            Property(e => e.Youtube)
                .HasColumnName("YOUTUBE")
                .HasMaxLength(255);
            
            Property(e => e.Instagram)
                .HasColumnName("INSTAGRAM")
                .HasMaxLength(255);
            
            Property(e => e.Site)
                .HasColumnName("SITE")
                .HasMaxLength(255);

            HasOptional(e => e.Endereco).WithOptionalDependent().Map(e => e.MapKey("ENDERECO_ID"));
        }
    }
}