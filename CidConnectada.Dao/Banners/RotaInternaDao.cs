using CidConnectada.Entities.Model.Banners;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Banners
{
    public class RotaInternaDao : BaseDao<RotaInterna, int, int, string>
    {
        public RotaInternaDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
        public override Dictionary<string, ListSortDirection> DefaultOrder => new Dictionary<string, ListSortDirection>
        {
            {
                "Nome", ListSortDirection.Ascending
            },
            {
                "Path", ListSortDirection.Ascending
            }
        };

        public override string[] DefaultIncludes => new string[1]
        {
            "BannerSet"
        };
    }
}