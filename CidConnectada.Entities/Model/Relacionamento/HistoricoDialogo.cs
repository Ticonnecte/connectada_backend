using System;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Enums;
using Zenite.Pi.Entities;

namespace CidConnectada.Entities.Model.Relacionamento
{
    public class HistoricoDialogo : BaseEntity<HistoricoDialogoKey>
    {
        public override HistoricoDialogoKey Key => new HistoricoDialogoKey
        {
            DialogoId = DialogoId,
            SequenciaIndex = SequenciaIndex
        };
        public string DialogoId { get; set; }
        public int SequenciaIndex { get; set; }

        public string Descricao { get; set; }
        public DateTime DhTransicao { get; set; }
        public string DhTransicaoStr { get; set; }
        public DialogoStatusEnum StatusEnum { get; set; }

        public Funcionario Funcionario { get; set; }
        public Dialogo Dialogo { get; set; }
    }
}