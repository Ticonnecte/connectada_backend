using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using CidConnectada.Entities.Model.Infos;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Dao.Infos
{
    public class CategoriaDao : MultiTenancyDao<Categoria, int, int, int, string>
    {
        public CategoriaDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }

        protected override int TenantValue
        {
            get => DaoHelper.GetTenantId() ?? base.TenantValue;
        }
        public override Dictionary<string, ListSortDirection> DefaultOrder => new Dictionary<string, ListSortDirection>
        {
            {
                "Nome", ListSortDirection.Ascending
            }
        }; 
    }
}