using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Comunicacao;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Comunicacao
{
    public class AgendaCulturalMap : EntityBaseMap<AgendaCultural, string>
    {
        public AgendaCulturalMap()
        {
            ToTable("AGENDA_CULTURAL", "comunicacao");

            Property(e => e.Key)
                .HasColumnName("ID")
                .HasMaxLength(128)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.Titulo)
                .HasColumnName("TITULO")
                .HasMaxLength(100)
                .IsRequired();

            Property(e => e.Descricao)
                .HasColumnName("DESCRICAO")
                .HasMaxLength(4000);

            Property(e => e.ImagemUrl)
                .HasColumnName("IMAGEM_URL")
                .HasMaxLength(255);

            Property(e => e.ImgHashCode)
                .HasColumnName("IMG_HASH_CODE");

            Property(e => e.DhEventoInicio)
                .HasColumnName("DH_EVENTO_INICIO");

            Property(e => e.DhEventoFinal)
                .HasColumnName("DH_EVENTO_FINAL");

            Property(e => e.Link)
                .HasColumnName("LINK")
                .HasMaxLength(255);
            
            Property(e => e.TenantKey)
                .HasColumnName("TENANT_ID");

        }
    }
}