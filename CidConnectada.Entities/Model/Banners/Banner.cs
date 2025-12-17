using CidConnectada.Entities.AWS;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Entities.Model.Organograma;
using System;
using System.Security.Cryptography;
using Zenite.Pi.Entities.Model.MultiTenancy;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Banners
{
    public class Banner : S3FileGeneric, IEquatable<Banner>
    {
        //public override string Key { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string ImagemUrl { 
            get {
                return _ImgUrl;
            }
            set {
                _ImgUrl = value;
            }
        }
        public bool EstaNaHome { get; set; }
        public RotaTipoEnum RotaTipoEnum { get; set; }
        public string Path { get; set; }
        public DateTime DhUltimoUpdate { get; set; }

        public RotaInterna RotaInterna { get; set; }
        public Usuario UltimoEditor { get; set; }
        public Prefeitura Prefeitura { get; set; }

        public bool Equals(Banner other)
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

        public override string GetS3Key(string extensao = null)
        {
            string result = "";
            if (!string.IsNullOrEmpty(extensao))
            {
                result = $"banners/{Key}/bannerImg.{extensao}";
            }
            else
            {
                result = base.GetS3Key();
            }
            return result;
        }

    }
}