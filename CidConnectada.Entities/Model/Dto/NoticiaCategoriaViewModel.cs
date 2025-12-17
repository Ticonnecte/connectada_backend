using System.Collections.Generic;
using CidConnectada.Entities.Model.Noticias;

namespace CidConnectada.Entities.Model.Dto
{
    public class NoticiaCategoriaViewModel
    {
        public NoticiaCategoria Categoria { get; set; }
        public List<Noticia> Noticias { get; set; }
    }
}