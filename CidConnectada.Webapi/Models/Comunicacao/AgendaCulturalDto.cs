using CidConnectada.Webapi.Models.Common;
using System;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Comunicacao
{
    public class AgendaCulturalDto : S3FileGenericDto
    {
        public string titulo { get; set; }
        public string descricao { get; set; }
        public DateTime dhEventoInicio { get; set; }
        public DateTime dhEventoFinal { get; set; }
        public string link { get; set; }
        public string extensaoImg
        {
            get {
                return _extensao;
            }
            set {
                _extensao = value;
            }
        }
        public string base64Img
        {
            get {
                return _base64;
            }
            set {
                _base64 = value;
            }
        }
        public string imagemUrl
        { 
            get {
                return _imgUrl;
            }
            set {
                _imgUrl = value;
            }
        }
    }
}