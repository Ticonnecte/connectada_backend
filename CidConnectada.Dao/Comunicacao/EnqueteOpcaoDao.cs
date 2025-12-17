using CidConnectada.Entities.Model.Comunicacao;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Comunicacao
{
    public class EnqueteOpcaoDao : BaseDao<EnqueteOpcao, EnqueteOpcaoKey, int, string>
    {
        public EnqueteOpcaoDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
        public override Dictionary<string, ListSortDirection> DefaultOrder
        {
            get => new Dictionary<string, ListSortDirection>
            {
                { "EnqueteId", ListSortDirection.Ascending },
                { "OpcaoIdx", ListSortDirection.Ascending }
            };
        }

        public override string[] DefaultIncludes
        {
            get => new string[1]
            {
                "Enquete"
            };
        }
    }
}