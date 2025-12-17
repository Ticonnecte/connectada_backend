using CidConnectada.Entities.Model.AWS;
using System.ComponentModel.DataAnnotations.Schema;

namespace CidConnectada.Entities.Model.Comercios
{
    public class Produto : S3FileGeneric
    {
        [NotMapped]
        public override int TenantKey { get => base.TenantKey; set => base.TenantKey = value; }
        public override string Key { get; set; }
        public string Nome { get; set; }

        public string Descricao { get; set; }

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

        public decimal Valor { get; set; }

        public Comercio Comercio { get; set; }

        public override string GetS3Key(string extensao = null)
        {
            string result = "";
            if (!string.IsNullOrEmpty(extensao))
            {
                result = $"comercios/{Comercio.Key}/produtos/{Key}.{extensao}";
            }
            else
            {
                result = base.GetS3Key();
            }
            return result;
        }
    }
}
