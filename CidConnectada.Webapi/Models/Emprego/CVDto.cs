using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CidConnectada.Entities.Model.Emprego;
using Zenite.Pi.Entities.Enums;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Emprego
{
    public class CVDto : BaseEntityMasterModel<int, CVExperienciaDto, CVExperienciaKey>
    {
        public int cidadaoId { get; set; }
        public string cidadaoNome { get; set; }
        [Required]
        public string funcao { get; set; }
        [Required]
        public string setorMercado { get; set; }
        public bool tornarPublico { get; set; }
        public IList<string> competenciaList { get; set; } = new List<string>();
        public IList<string> habilidadeList { get; set; } = new List<string>();
        public IList<CVExperienciaDto> experienciaList { get; set; } = new List<CVExperienciaDto>();

        public override ICollection<CVExperienciaDto> GetDetail1(EstadoCadastroEnum currentState)
        {
            return experienciaList;
        }

        public override void ClearDetails()
        {
            experienciaList.Clear();
        }
    }
}