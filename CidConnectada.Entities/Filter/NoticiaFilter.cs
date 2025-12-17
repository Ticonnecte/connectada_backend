using Zenite.Pi.Entities.Model.Search;

namespace CidConnectada.Entities.Filter
{
    public class NoticiaFilter : ContainsFilter
    {
        public int? noticiaCategoriaId { get; set; }
    }
}