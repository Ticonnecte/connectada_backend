using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CidConnectada.Webapi.Models.Emprego
{
    public class OfertaVagaDto : OfertaVagaBaseDto
    {
        public TimeSpan? horarioInicio { get; set; }
        public TimeSpan? horarioFinal { get; set; }
        [Required]
        public long enderecoId { get; set; }
        public string enderecoCompleto { get; set; }
        [Required]
        public string setorMercado { get; set; }

        public IList<string> competenciaList { get; set; } = new List<string>();
        public IList<string> habilidadeList { get; set; } = new List<string>();
    }
}