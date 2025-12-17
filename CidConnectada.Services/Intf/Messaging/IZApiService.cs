using System.Collections.Generic;
using System.Threading.Tasks;
using Zenite.Pi.Services;
using Zenite.Wa.Models.Zapi.Contact;
using Zenite.Wa.Models.Zapi.Instance;
using Zenite.Wa.Models.Zapi.Message;

namespace CidConnectada.Services.Intf.Messaging
{
    public interface IZApiService : IService
    {
        Task<ZApiMsgResultDto> SendMessageAsync(ZApiSendTextDto zap);
        Task<bool> PhoneExistsAsync(string phone);
        Task<bool> ConnectedAsync();
        Task<bool> DisconnectAsync();
        Task<ZApiQrCode64ResultDto> GetQrCodeBase64Async();
        //Task<IList<ZApiGetContactResultDto>> GetContactAsync(int? page, int? pageSize);
        Task<ZApiStatusInstanceDto> GetStatusAsync();
    }

    public class ZApiResultDto : ZApiMsgResultDto
    {
    }
}