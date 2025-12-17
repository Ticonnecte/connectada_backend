using CidConnectada.Entities.Model.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Account
{
    public class AspNetRolesDao : BaseDao<AspNetRoles, string, int, string>
    {
        public AspNetRolesDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
        public override Dictionary<string, ListSortDirection> DefaultOrder => new Dictionary<string, ListSortDirection>
        {
            { "Name", ListSortDirection.Ascending }
        };
    }
}