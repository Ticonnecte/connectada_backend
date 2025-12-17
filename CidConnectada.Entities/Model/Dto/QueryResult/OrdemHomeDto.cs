using System.ComponentModel.DataAnnotations;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Organograma
{
    public class OrdemHomeDto<TKey> : BaseEntityModel<TKey>
    {
        [Required]
        override public TKey key { get; set; }
        public string nome { get; set; }
        [Required]
        public byte? ordemHome { get; set; }
    }
}