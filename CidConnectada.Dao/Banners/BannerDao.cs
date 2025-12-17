using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using CidConnectada.Entities.Model.Banners;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Banners
{
    public class BannerDao : MultiTenancyDao<Banner, string, int, int, string>
    {
        public BannerDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }

        public override Dictionary<string, ListSortDirection> DefaultOrder => new Dictionary<string, ListSortDirection>
        {
            { "EstaNaHome", ListSortDirection.Ascending },
            { "Nome", ListSortDirection.Ascending }
        };


        protected override int TenantValue
        {
            get => DaoHelper.GetTenantId() ?? base.TenantValue;
        }

        public override string[] DefaultIncludes => new string[3]
        {
            "Prefeitura", "RotaInterna", "UltimoEditor"
        };
    }
}