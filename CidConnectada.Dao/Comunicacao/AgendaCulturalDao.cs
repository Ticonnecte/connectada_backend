using System;
using System.Collections.Generic;
using System.ComponentModel;
using CidConnectada.Entities.Model.Comunicacao;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Comunicacao
{
    public class AgendaCulturalDao : MultiTenancyDao<AgendaCultural, string, int, int, string>
    {
        public AgendaCulturalDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }

        public override Dictionary<string, ListSortDirection> DefaultOrder
        {
            get => new Dictionary<string, ListSortDirection>
            {
                { "DhEventoInicio", ListSortDirection.Descending },
                { "Titulo", ListSortDirection.Ascending }
            };
        }

        protected override int TenantValue
        {
            get => DaoHelper.GetTenantId() ?? base.TenantValue;
        }
    }
}