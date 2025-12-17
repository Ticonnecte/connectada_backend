using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Entities.Model.Local;
using CidConnectada.Entities.Model.Organograma;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Zenite.Pi.Entities.Model.MultiTenancy;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Relacionamento
{
    public class Dialogo : S3FileGeneric, IEquatable<Dialogo>
    {
        public override string Key { get; set; }
        public DateTime DhCriacao { get; set; }
        public string DhCriacaoStr { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
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
        public DialogoAssuntoEnum AssuntoDialogoEnum { get; set; }
        public DateTime? DataPrevistaExecuacao { get; set; }
        public DateTime? DataPrevistaFinalizacao { get; set; }
        public DialogoStatusEnum DialogoStatusEnum { get; set; }
        public Endereco Endereco { get; set; }

        public ISet<HistoricoDialogo> HistoricoDialogoSet { get; set; } = new HashSet<HistoricoDialogo>();

        public Secretaria Secretaria { get; set; }
        public Cidadao Cidadao { get; set; }

        public override string GetS3Key(string extensao = null)
        {
            string result = "";
            if (!string.IsNullOrEmpty(extensao))
            {
                result = $"dialogos/{Key}/dialogoImg.{extensao}";
            }
            else
            {
                result = base.GetS3Key();
            }
            return result;
        }

        public bool Equals(Dialogo other)
        {
            bool result;
            if (ReferenceEquals(other, null))
            {
                result = false;
            }
            else if (ReferenceEquals(other, this))
            {
                result = true;
            }
            else
            {
                result = EntityUtil.EqualsEntity(this, other);
            }

            return result;
        }
    }
}