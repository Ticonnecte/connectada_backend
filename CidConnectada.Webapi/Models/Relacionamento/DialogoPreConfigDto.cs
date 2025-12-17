using CidConnectada.Entities.Model.Enums;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Relacionamento
{
    public class DialogoPreConfigDto : BaseEntityModel<int>
    {
        public string nome { get; set; }
        public string iconeNome { get; set; }
        public string tituloPadrao { get; set; }
        public DialogoAssuntoEnum assuntoDialogoEnum { get; set; }
        public string assuntoDialogoEnumNome { get; set; }
        public string secretariaId { get; set; }
    }
}