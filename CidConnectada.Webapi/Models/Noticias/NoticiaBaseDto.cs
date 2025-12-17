using CidConnectada.Webapi.Models.Common;
using System;
using System.Collections.Generic;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Entities.Model.Dto
{
    public class NoticiaBaseDto : HtmlContentDto
    {
        public NoticiaBaseDto()
            : base()
        {
        }
        public NoticiaBaseDto(bool isView)
            : base(isView)
        {
        }
        public DateTime? dhCriacao { get; set; }
        public string autor { get; set; }
        public DateTime? dhUltimoUpdate { get; set; }
        public string editor { get; set; }
    }
}