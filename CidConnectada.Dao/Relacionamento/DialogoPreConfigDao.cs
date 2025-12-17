using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Relacionamento;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Relacionamento
{
    public class DialogoPreConfigDao : MultiTenancyDao<DialogoPreConfig, int,  int, int, string>
    {
        public DialogoPreConfigDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }

        public override Dictionary<string, ListSortDirection> DefaultOrder
        {
            get => new Dictionary<string, ListSortDirection>
            {
                { "Nome", ListSortDirection.Ascending }
            };
        }

        public override string[] DefaultIncludes { get => new string[1] { "Secretaria" }; }
        
        protected override int TenantValue
        {
            get => DaoHelper.GetTenantId() ?? base.TenantValue;
        }
    }
}