using CidConnectada.Entities.Model.Comercios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Comercios
{
    public class CategoriaTipoComercioDao: BaseDao<CategoriaTipoComercio, int, int, string>
    {
        public CategoriaTipoComercioDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
        public override Dictionary<string, ListSortDirection> DefaultOrder
        {
            get => new Dictionary<string, ListSortDirection>
            {
                { "Nome", ListSortDirection.Ascending }
            };
        }

        public override string[] DefaultIncludes => new string[1] {"TipoComercio"};

    }
}
