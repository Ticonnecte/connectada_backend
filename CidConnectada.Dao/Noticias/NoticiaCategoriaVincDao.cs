using CidConnectada.Entities.Model.Noticias;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Noticias
{
    public class NoticiaCategoriaVincDao : BaseDao<NoticiaCategoriaVinc, NoticiaCategoriaVincKey, int, string>
    {
        public NoticiaCategoriaVincDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
        public override Dictionary<string, ListSortDirection> DefaultOrder => new Dictionary<string, ListSortDirection>
        {
            { "NoticiaId", ListSortDirection.Ascending }
        };

        public override string[] DefaultIncludes => new string[2] { "Noticia", "NoticiaCategoria" };
    }
}