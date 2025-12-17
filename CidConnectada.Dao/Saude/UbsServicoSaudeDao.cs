using CidConnectada.Entities.Model.Saude;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Saude
{
    public class UbsServicoSaudeDao : BaseDao<UbsServicoSaude, UbsServicoSaudeKey, int, string>
    {
        public UbsServicoSaudeDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
        public override Dictionary<string, ListSortDirection> DefaultOrder => new Dictionary<string, ListSortDirection>
        {
            {
                "UbsId", ListSortDirection.Ascending
            }
        };

        public override string[] DefaultIncludes => new string[2]
        {
            "UnidadeBasicaSaude", "ServicoSaude"
        };
    }
}