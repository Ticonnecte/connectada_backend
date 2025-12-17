using CidConnectada.Entities.Model.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Account
{
    public class AspNetUserRolesDao : BaseDao<AspNetUserRoles, AspNetUserRolesKey, int, string>
    {
        public AspNetUserRolesDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
        public override Dictionary<string, ListSortDirection> DefaultOrder => new Dictionary<string, ListSortDirection>
        {
            { "AspNetUsers.UserName", ListSortDirection.Ascending }
        };
    }
}