using CidConnectada.Entities.Model.Comercios;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Comercios
{
    public class ProdutoMap: EntityBaseMap<Produto, string>
    {
        public ProdutoMap()
        {
            ToTable("PRODUTO", "comercio");
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
            
            Property(e => e.ImgUrl)
                .HasMaxLength(255)
                .HasColumnName("IMG_URL");

            Property(e => e.ImgHashCode)
                .HasColumnName("IMG_HASH_CODE");

            Property(e => e.Valor)
                .HasColumnName("VALOR");

            HasRequired(e => e.Comercio).WithMany(e => e.ProdutoSet).Map(m => m.MapKey("COMERCIO_ID"));
        }
    }
}
