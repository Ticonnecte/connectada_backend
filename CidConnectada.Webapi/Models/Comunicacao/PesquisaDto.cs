using System;
using System.ComponentModel.DataAnnotations;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Comunicacao
{
    public class PesquisaDto : BaseEntityModel<int>
    {
        [Required]
        public string nome { get; set; }

        [Required]
        public DateTime vigenciaInicio { get; set; }

        [Required]
        [NotPastDate]
        [MinDate("vigenciaInicio")]
        public DateTime vigenciaFinal { get; set; }

        [Required]
        public string googleFormsUrl { get; set; }
    }
}