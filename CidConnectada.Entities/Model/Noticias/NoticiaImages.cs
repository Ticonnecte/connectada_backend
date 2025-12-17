using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Relacionamento;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenite.Pi.Entities;

namespace CidConnectada.Entities.Model.Noticias
{
    public class NoticiaImages : HtmlImages
    {
        public NoticiaImages()
            : base() { }
        public NoticiaImages(int hashId, string parentId) : base(hashId, parentId)
        {
        }

        public Noticia Noticia { get; set; }
    }
}
