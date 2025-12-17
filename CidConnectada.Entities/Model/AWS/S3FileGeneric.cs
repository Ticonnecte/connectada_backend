using CidConnectada.Entities.AWS;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Zenite.Pi.Entities.Model.MultiTenancy;

namespace CidConnectada.Entities.Model.AWS
{
    public abstract class S3FileGeneric : MultiTenancy<string, int>, IS3File
    {
        [NotMapped]
        public string _Base64 {  get; set; }
        [NotMapped]
        public string _ImgUrl { get; set; }

        public int? ImgHashCode { get; set; }
        public virtual string GetS3Key(string extensao = null)
        {
            string result = null;
            if (string.IsNullOrEmpty(extensao) && !string.IsNullOrEmpty(_ImgUrl))
            {
                string s3Com = ".amazonaws.com/";
                int position = _ImgUrl.IndexOf(s3Com) + s3Com.Length; 
                result = _ImgUrl.Substring(position);
            }
            return result;
        }

        public virtual string GetS3Url(string baseUrl, string extensao)
        {
            return $"{baseUrl}{GetS3Key(extensao)}";
        }

        public virtual string GetExtension()
        {
            string result = "";
            string url = _ImgUrl;
            while (url.Last() != '.')
            {
                result = url.Last() + result;
                url = url.Substring(0, url.Length - 1);
            }
            return result;
        }

        public int CalculateHashCode()
        {
            int result = 0;
            if (!string.IsNullOrEmpty(this._Base64) && _Base64.Length >= 128)
            {
                result = _Base64.Substring(0, 128).GetHashCode();
            }
            else
            {
                result = ImgHashCode.HasValue ? ImgHashCode.Value : 0;
            }
            return result;
        }

    }
}
