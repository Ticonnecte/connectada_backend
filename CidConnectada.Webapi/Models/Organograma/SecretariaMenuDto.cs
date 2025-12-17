using System.ComponentModel.DataAnnotations;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Entities.Model.Organograma;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Organograma
{
    public class SecretariaMenuDto : BaseEntityModel<SecretariaMenuKey>
    {
        public override SecretariaMenuKey key
        {
            get => new SecretariaMenuKey { SecretariaId = secretariaId, OrdemIdx = ordemIdx };
        }
        public string secretariaId { get; set; }
        public byte ordemIdx { get; set; }
        public string iconeNome { get; set; }
        public string titulo { get; set; }
        public string secretariaNome { get; set; }
        public bool isActive { get; set; }
        [Required]
        public RotaTipoEnum rotaTipoEnum { get; set; }
        public string rotaTipoEnumNome { get; set; }
        [RequiredIfPropertyEquals("rotaTipoEnum", "Link_Interno")]
        public int rotaInternaId { get; set; }
        [RequiredIfPropertyEquals("rotaTipoEnum", "Link_Externo")]
        public string path { get; set; }
    }
}