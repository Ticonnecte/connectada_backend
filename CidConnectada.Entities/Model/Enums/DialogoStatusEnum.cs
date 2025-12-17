using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CidConnectada.Entities.Model.Enums
{
    public enum DialogoStatusEnum
    {
        Novo = 1,
        Em_Análise,
        Respondido,
        Aguardando_Agendamento,
        Em_Execução,
        Finalizado,
        Indeferido
    }
}
