using CidConnectada.Entities.Model.Comercios;
using CidConnectada.Services.Intf.AWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenite.Pi.Services;

namespace CidConnectada.Services.Intf.Comercios
{
    public interface IProdutoService: IS3FileServiceGeneric<Produto>
    {
    }
}
