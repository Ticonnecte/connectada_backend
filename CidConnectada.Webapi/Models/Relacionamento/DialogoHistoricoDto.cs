using System.Collections.Generic;
using CidConnectada.Entities.Model.Enums;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Relacionamento
{
    public class DialogoHistoricoDto : BaseEntityModel<string>
    {
        public DialogoAssuntoEnum DialogoAssuntoEnum { get; set; }
        public string assuntoDialogoEnumNome { get; set; }

        public DialogoStatusEnum dialogoStatusEnum { get; set; }
        public string dialogoStatusEnumNome { get; set; }
        public IList<HistoricoDialogoViewDto> historicoList { get; set; }
    }
}