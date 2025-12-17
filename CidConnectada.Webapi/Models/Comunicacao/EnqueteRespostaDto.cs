using System.Collections.Generic;

namespace CidConnectada.Webapi.Models.Comunicacao
{
    public class EnqueteRespostaDto
    {
        public int enqueteId { get; set; }
        public IList<byte> opcoes { get; set; } = new List<byte>();
    }
}