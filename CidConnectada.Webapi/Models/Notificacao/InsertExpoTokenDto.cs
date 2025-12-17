using System;

namespace CidConnectada.Webapi.Models.Noticias
{
    public class InsertExpoTokenDto
    {
        [ValidateGuid]
        public Guid deviceId { get; set; }
        public string token { get; set; }
    }
}