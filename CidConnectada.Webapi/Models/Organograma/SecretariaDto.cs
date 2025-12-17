using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper.Configuration.Annotations;
using CidConnectada.Entities.Model.Organograma;
using Zenite.Pi.Entities.Enums;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Organograma
{
    public class SecretariaDto : BaseEntityMasterModel<string, SecretariaMenuDto, SecretariaMenuKey>
    {
        public string iconeNome { get; set; }
        public byte? ordemHome { get; set; }
        [Required]
        public string nome { get; set; }
        public string nomeSecretario { get; set; }
        public bool isActive { get; set; }
        [Ignore]
        public IList<SecretariaMenuDto> secretariaMenuList { get; set; } = new List<SecretariaMenuDto>();

        public override void ClearDetails()
        {
            secretariaMenuList.Clear();
        }

        public override ICollection<SecretariaMenuDto> GetDetail1(EstadoCadastroEnum currentState)
        {
            return secretariaMenuList;
        }
    }
}