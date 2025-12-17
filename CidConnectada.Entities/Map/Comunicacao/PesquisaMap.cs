using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Comunicacao;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Comunicacao
{
    public class PesquisaMap : EntityBaseMap<Pesquisa, int>
    {
        public PesquisaMap()
        {
            ToTable("PESQUISA", "comunicacao");

            Property(e => e.Key)
                .HasColumnName("ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Nome)
                .HasColumnName("NOME")
                .HasMaxLength(100)
                .IsRequired();

            Property(e => e.VigenciaInicio)
                .HasColumnName("VIGENCIA_INICIO");

            Property(e => e.VigenciaFinal)
                .HasColumnName("VIGENCIA_FINAL");

            Property(e => e.GoogleFormsUrl)
                .HasColumnName("GOOGLE_FORMS_URL")
                .HasMaxLength(400)
                .IsRequired();

            Property(e => e.TenantKey)
                .HasColumnName("TENANT_ID");
        }
    }
}