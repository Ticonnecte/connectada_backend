using CidConnectada.Entities.Model.Enums;
using CidConnectada.Webapi.Models.Common;
using System;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Banners
{
    public class BannerViewDto : S3FileGenericDto
    {
        public string nome { get; set; }
        public string descricao { get; set; }
        public bool estaNaHome { get; set; }
        public string path { get; set; }
        public RotaTipoEnum rotaTipoEnum { get; set; }
        public string rotaTipoEnumNome { get; set; }
        public string imagemUrl { get; set; }
        //    get {
        //        return _imgUrl;
        //    }
        //    set {
        //        _imgUrl = value;
        //    }
        //}
        public DateTime dhUltimoUpdate { get; set; }
        public string ultimoEditor { get; set; }
    }
}