using CidConnectada.Entities.Model.Saude;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Saude
{
    public class ServicoSaudeDao : BaseDao<ServicoSaude, int, int, string>
    {
        public ServicoSaudeDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
        public override Dictionary<string, ListSortDirection> DefaultOrder => new Dictionary<string, ListSortDirection>
        {
            {
                "Nome", ListSortDirection.Ascending
            }
        };
    }
}