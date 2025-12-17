using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Local;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Local
{
    public class EnderecoMap : EntityBaseMap<Endereco, long>
    {
        public EnderecoMap()
        {
            ToTable("ENDERECO", "local");

            Property(e => e.Key)
                .HasColumnName("ID")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Rua)
                .HasColumnName("RUA")
                .HasMaxLength(255);

            Property(e => e.Numero)
                .HasColumnName("NUMERO")
                .HasMaxLength(15);

            Property(e => e.Complemento)
                .HasColumnName("COMPLEMENTO")
                .HasMaxLength(100);

            Property(e => e.Cep)
                .HasColumnName("CEP")
                .HasMaxLength(25);

            Property(e => e.Bairro)
                .HasColumnName("BAIRRO")
                .HasMaxLength(50);

            Property(e => e.Coordenadas)
                .HasColumnName("COORDENADAS");

            Property(e => e.GoogleMapsPlaceId)
                .HasColumnName("GOOGLE_MAPS_PLACE_ID");

            HasRequired(e => e.Cidade).WithMany(e => e.EnderecoSet).Map(e => e.MapKey("CIDADE_ID"));
        }
    }
}