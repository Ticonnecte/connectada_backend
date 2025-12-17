using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CidConnectada.Entities.Model.Dto.Location;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Webapi.Models.Common;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Saude
{
    public class UbsBaseDto : S3FileGenericDto
    {
        [Required]
        public string nome { get; set; }
        [Required]
        public string codigoCNES { get; set; }

        public string enderecoCompleto { get; set; }
        public UbsTipoEnum tipoUnidadeEnum { get; set; }
        public UbsPorteEnum porteEnum { get; set; }
        public UbsRegiaoAbrangenciaEnum regiaoAbrangenciaEnum { get; set; }
        public bool acessibilidade { get; set; }
        public TimeSpan horarioFuncionamentoInicio { get; set; }
        public TimeSpan horarioFuncionamentoFinal { get; set; }
        public int? capacidadeAtendimentoDia { get; set; }
        public UbsSituacaoEnum situacaoEnum { get; set; }
        public IList<DetailDto> especialidadeMedicaList { get; set; }

        public IList<DetailDto> servicoSaudeList { get; set; }
    }
}
