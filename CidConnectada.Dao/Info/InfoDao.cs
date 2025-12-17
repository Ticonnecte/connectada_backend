using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using CidConnectada.Entities.Model.Infos;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Dao.Infos { 
     
    public class InfoDao : MultiTenancyDao<Info, string, int, int, string>
    {
        public InfoDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }

        protected override int TenantValue
        {
            get => DaoHelper.GetTenantId() ?? base.TenantValue;
        }

        public override string[] DefaultIncludes => new string[2]
        {
            "Categoria",
            "InfoImagesSet"
        };

        public override Dictionary<string, ListSortDirection> DefaultOrder => new Dictionary<string, ListSortDirection>
        {
            {
                "Lead", ListSortDirection.Ascending
            }
        };

        //public override Info Add(Info entity)
        //{
        //    DbEntityEntry<Categoria> catEntry = Context.Entry<Categoria>(entity.Categoria);
        //    Categoria cat = catEntry.Entity;
        //    log.Info(cat);
        //    return base.Add(entity);
        //}
    }
}

