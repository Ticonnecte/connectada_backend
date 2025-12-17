using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Infos;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Infos
{
    public class InfoMap : EntityBaseMap<Info, string>
    {
        public InfoMap()
        {
            ToTable("INFO", "info");

            Property(e => e.Key)
                .HasColumnName("ID")
                .HasMaxLength(128)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.Lead)
                .HasColumnName("LEAD")
                .HasMaxLength(400);

            Property(e => e.Conteudo)
                .HasColumnName("CONTEUDO");

            Property(e => e.FotoCapaUrl)
                .HasColumnName("FOTO_CAPA_URL");

            Property(e => e.TenantKey)
                .HasColumnName("TENANT_ID");

            Property(e => e.Ativa)
                .HasColumnName("ATIVA");

            Property(e => e.ImgHashCode)
                .HasColumnName("IMG_HASH_CODE");

            //Property(e => e.CreationUser)
            //    .HasColumnName("USUARIO_INS");
            //Property(e => e.CreationDate)
            //    .HasColumnName("DH_CRIACAO");
            //Property(e => e.UpdateUser)
            //    .HasColumnName("USUARIO_UPD");
            //Property(e => e.UpdateDate)
            //    .HasColumnName("DH_ULTIMO_UPD");
            HasRequired(e => e.Categoria).WithMany(e => e.InfoSet).Map(e => e.MapKey("CATEGORIA_ID"));
        }
    }
}