using CidConnectada.Entities.Model.Local;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Local
{
    public class EstadoDao : BaseDao<Estado, int, int, string>
    {
        public EstadoDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
        public override Dictionary<string, ListSortDirection> DefaultOrder => new Dictionary<string, ListSortDirection>
        {
            { "Nome", ListSortDirection.Ascending }
        };
    }
}