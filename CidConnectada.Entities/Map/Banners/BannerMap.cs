using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Banners;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Banners
{
    public class BannerMap : EntityBaseMap<Banner, string>
    {
        public BannerMap()
        {
            ToTable("BANNER", "banner");

            Property(e => e.Key)
                .HasColumnName("ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.Nome)
                .HasColumnName("NOME")
                .HasMaxLength(50);

            Property(e => e.Descricao)
                .HasColumnName("DESCRICAO")
                .HasMaxLength(4000);

            Property(e => e.ImagemUrl)
                .HasColumnName("IMAGEM_URL");

            Property(e => e.ImgHashCode)
                .HasColumnName("IMG_HASH_CODE");

            Property(e => e.EstaNaHome)
                .HasColumnName("ESTAH_NA_HOME");

            Property(e => e.RotaTipoEnum)
                .HasColumnName("ROTA_TIPO_ENUM");

            Property(e => e.TenantKey)
                .HasColumnName("TENANT_ID");

            Property(e => e.Path)
                .HasColumnName("PATH_OTHERS")
                .HasMaxLength(1000);

            Property(e => e.DhUltimoUpdate)
                .HasColumnName("DH_ULTIMO_UPD");

            HasRequired(e => e.Prefeitura).WithMany().HasForeignKey(e => e.TenantKey);
            HasOptional(e => e.RotaInterna).WithMany(e => e.BannerSet).Map(e => e.MapKey("ID_ROTA_INTERNA"));
            HasOptional(e => e.UltimoEditor).WithMany(e => e.BannerSet).Map(e => e.MapKey("ULTIMO_EDITOR_ID"));
        }
    }
}