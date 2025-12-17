using System;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Noticias
{
    public class ExpoNotificationTokenDto : BaseEntityModel<string>
    {
        //   public int Id { get; set; }
        public int UserId { get; set; }
        public string DeviceId { get; set; }
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}