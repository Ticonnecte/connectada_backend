using System.ComponentModel.DataAnnotations;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Entities.Model.Relacionamento;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Relacionamento
{
    public class HistoricoDialogoDto : BaseEntityModel<HistoricoDialogoKey>
    {
        public override HistoricoDialogoKey key => new HistoricoDialogoKey
        {
            DialogoId = dialogoId,
            SequenciaIndex = sequenciaIndex
        };
        public string dialogoId { get; set; }
        public int sequenciaIndex { get; set; }

        [Required]
        public string descricao { get; set; }
        [Required]
        public DialogoStatusEnum statusEnum { get; set; }
    }
}