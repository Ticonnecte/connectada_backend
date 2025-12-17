using CidConnectada.Entities.Model.Enums;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Noticias
{
    public class NoticiaCategoriaDto : BaseEntityModel<int>
    {
        public string nome { get; set; }
        public CorEnum cor { get; set; }
        public string corNome { get; set; }
        public string descricao { get; set; }
        public string iconeNome { get; set; }
    }
}