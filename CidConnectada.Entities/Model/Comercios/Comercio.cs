using CidConnectada.Entities.AWS;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Local;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Zenite.Pi.Entities.Model.MultiTenancy;

namespace CidConnectada.Entities.Model.Comercios
{
    public class Comercio: S3FileGeneric
    {
        public override string Key { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string NumeroWhatsApp { get; set; }
        public byte OrdemHome { get; set; }
        public bool IsActive { get; set; }
        public TimeSpan AbreAs { get; set; }
        public TimeSpan FechaAs { get; set; }
        public string ImgUrl
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
        public Cidadao Cidadao { get; set; }
        public TipoComercio TipoComercio { get; set; }
        public Endereco Endereco { get; set; }

        public ISet<ComercioCategoriaVinculo> ComercioCategoriaVinculoSet { get; set; } = new HashSet<ComercioCategoriaVinculo>();

        public ISet<Produto> ProdutoSet { get; set; } = new HashSet<Produto>();

        public override string GetS3Key(string extensao = null)
        {
            string result = "";
            if (!string.IsNullOrEmpty(extensao))
            {
                result = $"comercios/{Key}/card.{extensao}";
            }
            else
            {
                result = base.GetS3Key();
            }
            return result;
        }
    }
}
