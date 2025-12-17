using CidConnectada.Entities.Model.Local;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Local
{
    public class EnderecoDao : BaseDao<Endereco, long, int, string>
    {
        public EnderecoDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
        public override Dictionary<string, ListSortDirection> DefaultOrder => new Dictionary<string, ListSortDirection>
        {
            { "Rua", ListSortDirection.Ascending }
        };

        public override string[] DefaultIncludes => new string[1] { "Cidade.Estado" };
    }
}