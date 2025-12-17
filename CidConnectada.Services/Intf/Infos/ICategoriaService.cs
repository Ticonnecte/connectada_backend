using CidConnectada.Entities.Model.Infos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenite.Pi.Services;

namespace CidConnectada.Services.Intf.Infos
{
    public interface ICategoriaService : ICadastroService<Categoria, int>
    {
        Task<IList<Categoria>> GetAtivasAsync();
    }
}
