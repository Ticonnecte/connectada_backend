using System;
using System.Linq;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Entities.Model.Dto.Location
{
    public class EnderecoDto : BaseEntityModel<long>
    {
        public string rua { get; set; }
        public string numero { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string cep { get; set; }
        public string enderecoCompleto => String.Join(", ", new[]
        {
            rua, numero, bairro, $"{cidadeNome} - {estadoSigla}", cep
        }.Where(s => !String.IsNullOrWhiteSpace(s)));
        public LocationDto coordenadas { get; set; }
        public string googleMapsPlaceId { get; set; }
        public int cidadeId { get; set; }
        public string cidadeNome { get; set; }
        public string estadoSigla { get; set; }
    }
}