using System;
using CidConnectada.Entities.Model.Enums;

namespace CidConnectada.Webapi.Models.Relacionamento
{
    public class HistoricoDialogoViewDto
    {
        public string dialogoId { get; set; }
        public int sequenciaIndex { get; set; }

        public string descricao { get; set; }
        public DateTime dhTransicao { get; set; }
        public DialogoStatusEnum statusEnum { get; set; }
        public string statusEnumNome { get; set; }
    }
}