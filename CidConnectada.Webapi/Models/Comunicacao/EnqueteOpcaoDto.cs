using System.ComponentModel.DataAnnotations;
using CidConnectada.Entities.Model.Comunicacao;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Comunicacao
{
    public class EnqueteOpcaoDto : BaseEntityModel<EnqueteOpcaoKey>
    {
        public override EnqueteOpcaoKey key
        {
            get => new EnqueteOpcaoKey
            {
                EnqueteId = enqueteId,
                OpcaoIdx = opcaoIdx
            };
        }

        public int enqueteId { get; set; }
        [Required]
        public byte opcaoIdx { get; set; }
        [Required]
        public string texto { get; set; }
    }
}