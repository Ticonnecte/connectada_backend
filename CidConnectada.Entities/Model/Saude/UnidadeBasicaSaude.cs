using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Entities.Model.Local;
using CidConnectada.Entities.Model.Organograma;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Security.Cryptography;
using Zenite.Pi.Entities.Model.MultiTenancy;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Saude
{
    public class UnidadeBasicaSaude : S3FileGeneric, IEquatable<UnidadeBasicaSaude>
    {

        public override string Key { get; set; }
        public string Nome { get; set; }
        public string CodigoCNES { get; set; }
        public UbsTipoEnum TipoUnidadeEnum { get; set; }
        public UbsPorteEnum PorteEnum { get; set; }
        public Endereco Endereco { get; set; }
        public UbsRegiaoAbrangenciaEnum RegiaoAbrangenciaEnum { get; set; }
        public decimal? AreaTotal { get; set; }
        public int? NumeroSalas { get; set; }
        public bool Acessibilidade { get; set; }
        public int? NumEquipeSaudeFamilia { get; set; }
        public int? NumProfissionais { get; set; }
        public TimeSpan HorarioFuncionamentoInicio { get; set; }
        public TimeSpan HorarioFuncionamentoFinal { get; set; }
        public int? CapacidadeAtendimentoDia { get; set; }
        public string ResponsavelNome { get; set; }
        public string ResponsavelWhatsApp { get; set; }
        public string VinculacaoAdmnistrativa { get; set; }
        public UbsSituacaoEnum SituacaoEnum { get; set; }
        
        public string ImagemUrl
        {
            get
            {
                return _ImgUrl;
            }
            set
            {
                _ImgUrl = value;
            }
        }

        public ISet<UbsEspecialidadeMedica> UbsEspecialidadeMedicaSet { get; set; }
        public ISet<UbsServicoSaude> UbsServicoSaudeSet { get; set; }

        public Prefeitura Prefeitura { get; set; }

        public override string GetS3Key(string extensao = null)
        {
            string result = "";
            if (!string.IsNullOrEmpty(extensao))
            {
                result = $"ubs/{Key}/ubsImg.{extensao}";
            }
            else
            {
                result = base.GetS3Key();
            }
            return result;
        }

        public bool Equals(UnidadeBasicaSaude other)
        {
            bool result;
            if (ReferenceEquals(other, null))
                result = false;
            else if (ReferenceEquals(other, this))
                result = true;
            else
                result = EntityUtil.EqualsEntity(this, other);
            return result;
        }
    }
}
