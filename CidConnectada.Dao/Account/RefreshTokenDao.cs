using CidConnectada.Entities.Model.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Account
{
    public class RefreshTokenDao : BaseDao<RefreshToken, Guid, int, string>
    {

        public RefreshTokenDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }

        public override Dictionary<string, ListSortDirection> DefaultOrder => new Dictionary<string, ListSortDirection>
        {
            {
                "DataEmissaoUtd", ListSortDirection.Ascending
            }
        };

        public override string[] DefaultIncludes =>
            ConcatanateIncludes(base.DefaultIncludes, new string[2]
            {
                "User.AspNetUsers", "Device"
            });
    }
}