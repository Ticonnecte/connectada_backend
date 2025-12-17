using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zenite.Pi.Entities.Enums;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Comercios
{
    public class TipoComercioDto : BaseEntityMasterModel<int, CategoriaTipoComercioDto, int>
    {
        [Required]
        public string nome { get; set; }
        public byte? ordemHome { get; set; }
        public string iconeNome { get; set; }
        public bool isActive { get; set; }

        public IList<CategoriaTipoComercioDto> categorias { get; set; } = new List<CategoriaTipoComercioDto>();

        public override void ClearDetails()
        {
            categorias.Clear();
        }

        public override ICollection<CategoriaTipoComercioDto> GetDetail1(EstadoCadastroEnum currentState)
        {
            return categorias;
        }
    }
}
