using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Emprego
{
    public class CVViewDto : BaseEntityModel<int>
    {
        public string nome { get; set; }
        public string experiencia { get; set; }
        public string funcao { get; set; }
        public string setorMercado { get; set; }
    }
}