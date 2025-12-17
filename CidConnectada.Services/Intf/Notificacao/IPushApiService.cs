using System.Threading.Tasks;
using Expo.Server.Models;
using Zenite.Pi.Services;

namespace CidConnectada.Services.Intf.Notificacao
{
    public interface IPushApiService : IService
    {
        Task<PushTicketResponse> PushSendAsync(PushTicketRequest pushTicketRequest);
        Task<PushResceiptResponse> PushGetReceiptsAsync(PushReceiptRequest pushReceiptRequest);
    }
}