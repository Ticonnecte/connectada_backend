using System;
using System.ComponentModel.DataAnnotations.Schema;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Common
{
    public class S3FileGenericDto : BaseEntityModel<string>
    {
        protected string _base64 { get; set; }
        
        protected string _imgUrl { get; set; }

        protected string _extensao { get; set; }

        public bool CanSetImgUrl()
        {
            return isNew && !String.IsNullOrEmpty(_base64) && !String.IsNullOrEmpty(_extensao);
        }

        public void SetImgUrl(string imgUrl)
        {
            _imgUrl = imgUrl;
        }

        public string GetBase64()
        {
            return _base64;
        }

        public string GetExtension() { return _extensao; }

        public int CalculateHashCode()
        {
            int result = 0;
            if (!string.IsNullOrEmpty(this._base64) && _base64.Length >= 128)
            {
                result = _base64.Substring(0, 128).GetHashCode();
            }
            return result;
        }

        public bool CanUpdate(int hashCode)
        {
            return isNew || (CalculateHashCode() != hashCode && !string.IsNullOrEmpty(GetBase64()) && !string.IsNullOrEmpty(GetExtension()));
        }
    }
}