using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Common
{
    public class DetailDto : BaseEntityModel<int>
    {
        public string nome { get; set; }
        public string descricao { get; set; }
    }
}