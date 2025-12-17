using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using CidConnectada.Entities.Model.Dto;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Noticias
{
    public class NoticiaDto : NoticiaViewDto
    {
        public NoticiaDto()
            : base()
        {
        }
        public NoticiaDto(bool isView)
            : base(isView)
        {
        }
        public bool enviarWhatsApp { get; set; }
    }
    
    public class NoticiaViewDto : NoticiaBaseDto
    {
        public NoticiaViewDto()
            : base()
        {
        }
        public NoticiaViewDto(bool isView)
            : base(isView)
        {
        }

        public IList<NoticiaCategoriaDto> categorias { get; set; }
    }
}