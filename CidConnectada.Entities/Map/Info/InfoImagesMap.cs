using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Infos;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Infos
{
    public class InfoImagesMap : EntityBaseMap<InfoImages, HtmlImagesKey>
    {
        public InfoImagesMap()
        {
            ToTable("INFO_IMAGES", "info");

            Property(e => e.HashId)
                .HasColumnName("HASH_ID")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.ParentId)
                .HasColumnName("INFO_ID")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.ImgUrl)
                .HasMaxLength(1000)
                .IsRequired()
                .HasColumnName("IMG_URL");

            HasRequired(e => e.Info).WithMany(e => e.InfoImagesSet).HasForeignKey(e => e.ParentId);
        }

        protected override void DefineHasKey()
        {
            HasKey(entity => new
            {
                entity.HashId,
                entity.ParentId
            });
        }

    }
}
