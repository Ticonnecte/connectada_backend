using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Comercios
{
    public class CategoriaTipoComercioDto: BaseEntityModel<int>
    {
        public string nome { get; set; }

        public int tipoId { get; set; }
    }
}
