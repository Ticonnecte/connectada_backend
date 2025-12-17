using AutoMapper.Configuration.Annotations;
using CidConnectada.Entities.Model.Comunicacao;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zenite.Pi.Entities.Enums;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Comunicacao
{
    public class EnqueteDto : BaseEntityMasterModel<int, EnqueteOpcaoDto, EnqueteOpcaoKey>
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
        public bool isMultiVal { get; set; }
        public int metaRespostas { get; set; }
        [Required]
        public string pergunta { get; set; }
        [Ignore]
        [Required]
        public IList<EnqueteOpcaoDto> enqueteOpcaoList { get; set; } = new List<EnqueteOpcaoDto>();
        public EnqueteRespostaDto enqueteResposta { get; set; }

        public override void ClearDetails()
        {
            enqueteOpcaoList.Clear();
        }

        public override ICollection<EnqueteOpcaoDto> GetDetail1(EstadoCadastroEnum currentState)
        {
            return enqueteOpcaoList;
        }
    }
}