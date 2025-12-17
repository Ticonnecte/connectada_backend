using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Banners;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Banners
{
    public class RotaInternaMap : EntityBaseMap<RotaInterna, int>
    {
        public RotaInternaMap()
        {
            ToTable("ROTA_INTERNA", "banner");

            Property(e => e.Key)
                .HasColumnName("ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Nome)
                .HasColumnName("NOME")
                .HasMaxLength(500);

            Property(e => e.Path)
                .HasColumnName("PATH")
                .HasMaxLength(500);

            Property(e => e.EhBanner)
                .HasColumnName("EH_BANNER");
            
            Property(e => e.EhSecretaria)
                .HasColumnName("EH_SECRETARIA");
        }
    }
}