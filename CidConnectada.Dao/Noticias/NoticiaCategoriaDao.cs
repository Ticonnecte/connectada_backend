using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using CidConnectada.Entities.Model.Noticias;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Noticias
{
    public class NoticiaCategoriaDao : MultiTenancyDao<NoticiaCategoria, int, int, int, string>
    {
        public NoticiaCategoriaDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }

        public override Dictionary<string, ListSortDirection> DefaultOrder => new Dictionary<string, ListSortDirection>
        {
            {
                "Nome", ListSortDirection.Ascending
            }
        };

        protected override int TenantValue
        {
            get => DaoHelper.GetTenantId() ?? base.TenantValue;
        }

        public override string[] DefaultIncludes => new string[1]
        {
            "Prefeitura"
        };
    }
}