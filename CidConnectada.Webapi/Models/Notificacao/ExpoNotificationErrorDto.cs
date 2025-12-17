using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Noticias
{
    public class ExpoNotificationErrorDto : BaseEntityModel<string>
    {
        public int ExpoNotificationTokenId { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
    }
}