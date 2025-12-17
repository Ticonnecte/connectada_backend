using CidConnectada.Entities.Model.Local;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Local
{
    public class CidadeDao : BaseDao<Cidade, int, int, string>
    {
        public CidadeDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
        public override Dictionary<string, ListSortDirection> DefaultOrder => new Dictionary<string, ListSortDirection>
        {
            { "Nome", ListSortDirection.Ascending }
        };

        public override string[] DefaultIncludes => new string[1] { "Estado" };
    }
}