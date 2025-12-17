using CidConnectada.Entities.Model.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Account
{
    public class DeviceDao : BaseDao<Device, Guid, int, string>
    {

        public DeviceDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }

        public override Dictionary<string, ListSortDirection> DefaultOrder => new Dictionary<string, ListSortDirection>
        {
            {
                "Name", ListSortDirection.Ascending
            }
        };

        public override string[] DefaultIncludes =>
            ConcatanateIncludes(base.DefaultIncludes, new string[1]
            {
                "RefreshTokenSet.User.AspNetUsers"
            });
    }
}