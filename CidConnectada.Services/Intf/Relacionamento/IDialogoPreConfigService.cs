using CidConnectada.Entities.Model.Emprego;
using CidConnectada.Entities.Model.Relacionamento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenite.Pi.Services;

namespace CidConnectada.Services.Intf.Relacionamento
{
    public interface IDialogoPreConfigService: ICadastroService<DialogoPreConfig, int>
    {
    }
}
