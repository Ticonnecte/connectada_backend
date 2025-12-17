using System;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Noticias
{
    public class ExpoNotificationStatusDto : BaseEntityModel<string>
    {
        // public int Id { get; set; }
        public int NotificationId { get; set; }
        public int ExpoNotificationTokenId { get; set; }
        public DateTime SendAt { get; set; }
        public string ExpoId { get; set; }
    }
}