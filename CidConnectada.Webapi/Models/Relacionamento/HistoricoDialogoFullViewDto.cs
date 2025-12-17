using System;
using System.ComponentModel.DataAnnotations;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Entities.Model.Relacionamento;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Relacionamento
{
    public class HistoricoDialogoFullViewDto : BaseEntityModel<HistoricoDialogoKey>
    {
        public override HistoricoDialogoKey key => new HistoricoDialogoKey
        {
            DialogoId = dialogoId,
            SequenciaIndex = sequenciaIndex
        };
        public string dialogoId { get; set; }
        public int sequenciaIndex { get; set; }
        //public string titulo { get; set; }
        public string descricao { get; set; }
        //public DialogoAssuntoEnum assuntoDialogoEnum { get; set; }

        //public string assuntoDialogoNome { get; set; }
        //public string secretariaNome { get; set; }
        //public string historico { get; set; }
        public DateTime dhTransicao { get; set; }
        public DialogoStatusEnum statusEnum { get; set; }
        public string statusEnumNome { get; set; }
    }
}