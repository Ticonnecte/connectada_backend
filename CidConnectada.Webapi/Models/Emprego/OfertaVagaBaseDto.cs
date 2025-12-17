using System.ComponentModel.DataAnnotations;
using CidConnectada.Entities.Model.Enums;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Emprego
{
    public class OfertaVagaBaseDto : BaseEntityModel<long>
    {
        public string nomeEmpresa { get; set; }
        public byte experienciaMin { get; set; }
        [Required]
        public string funcao { get; set; }
        public int faixaSalarialId { get; set; }
        public decimal? faixaSalarialValorMin { get; set; }
        public decimal? faixaSalarialValorMax { get; set; }
        public OfertaVagaStatusEnum statusEnum { get; set; }
        public string statusEnumNome { get; set; }
    }
}