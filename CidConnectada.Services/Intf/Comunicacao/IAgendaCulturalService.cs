using CidConnectada.Entities.Model.Comunicacao;
using CidConnectada.Services.Intf.AWS;
using Zenite.Pi.Services;

namespace CidConnectada.Services.Intf.Comunicacao
{
    public interface IAgendaCulturalService : IS3FileServiceGeneric<AgendaCultural>
    {
    }
}