using System.Collections.Generic;

namespace CidConnectada.Entities.Model.Dto
{
    public class EnqueteResultadoDto
    {
        public int key { get; set; }
        public string nome { get; set; }
        public int totalVotos { get; set; }
        public IList<EnqueteOpcaoResultadoDto> resultado { get; set; } = new List<EnqueteOpcaoResultadoDto>();
    }

    public class EnqueteOpcaoResultadoDto
    {
        public int opcaoIdx { get; set; }
        public string texto { get; set; }
        public int qtdeVotos { get; set; }
    }
}