using Zenite.Pi.Entities.Model.Search;

namespace CidConnectada.Entities.Filter
{
    public class MesContainsFilter : ContainsFilter
    {
        public int? ano { get; set; }
        public int? mes { get; set; }
    }
}
