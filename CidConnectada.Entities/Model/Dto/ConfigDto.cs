using System.Collections.Generic;

namespace CidConnectada.Entities.Model.Dto
{
    public class ConfigDto
    {
        public ConfigDto(int tennantId)
        {
            tennantId = tenantId;
        }
        public int tenantId { get; set; } = 1;
        public int logoHeight { get; set; } = 110;
        public string clientName { get; set; } = "CONNECTADA";
        public string systemIconName { get; set; } = "";
        public string systemName { get; set; } = "CONNECTADA";
        public string systemTitleName { get; set; } = "PREFEITURA DIGITAL";
        public int notificationTimeout { get; set; } = 5000;
    }
}