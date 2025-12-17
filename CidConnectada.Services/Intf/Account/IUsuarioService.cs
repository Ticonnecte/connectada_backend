using System.Collections.Generic;
using System.Threading.Tasks;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Identity;
using CidConnectada.Entities.Model.Organograma;

namespace CidConnectada.Services.Intf.Account
{
    public interface IUsuarioService : IUsuarioGenericService<Usuario>
    {
        IList<Usuario> GetWhatsAppEnabled();
        Task<IList<Usuario>> GetWhatsAppEnabledAsync();
    }
}