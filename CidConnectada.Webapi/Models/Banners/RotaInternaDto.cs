using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Banners
{
    public class RotaInternaDto : BaseEntityModel<int>
    {
        public string nome { get; set; }
        public string path { get; set; }
    }
}