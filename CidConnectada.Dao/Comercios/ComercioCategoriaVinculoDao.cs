using CidConnectada.Entities.Model.Comercios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Comercios
{
    public class ComercioCategoriaVinculoDao: BaseDao<ComercioCategoriaVinculo, ComercioCategoriaVinculoKey, int, string>
    {
        public ComercioCategoriaVinculoDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
        public override Dictionary<string, ListSortDirection> DefaultOrder
        {
            get => new Dictionary<string, ListSortDirection>
            {
                { "Comercio.OrdemHome", ListSortDirection.Ascending }
            };
        }

    }
}
