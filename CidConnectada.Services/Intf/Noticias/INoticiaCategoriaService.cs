using System;
using System.Collections.Generic;
using CidConnectada.Entities.Model.Noticias;
using Zenite.Pi.Services;

namespace CidConnectada.Services.Intf.Noticias
{
    public interface INoticiaCategoriaService : ICadastroService<NoticiaCategoria, int>
    {
        IList<NoticiaCategoria> GetByNoticiaId(string idNoticia);
    }
}