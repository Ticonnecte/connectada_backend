using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using CidConnectada.Entities.Model.Comercios;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Dao.Comercios
{
    public class ComercioDao: MultiTenancyDao<Comercio, string, int, int, string>
    {
        public ComercioDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }

        public override Dictionary<string, ListSortDirection> DefaultOrder
        {
            get => new Dictionary<string, ListSortDirection>
            {
                { "OrdemHome", ListSortDirection.Ascending }
            };
        }

        public override string[] DefaultIncludes => new string[4] { "TipoComercio", "Endereco", "Cidadao", "ComercioCategoriaVinculoSet.Categoria" };

        protected override int TenantValue
        {
            get => DaoHelper.GetTenantId() ?? base.TenantValue;
        }
        
        public override IQueryable<Comercio> Where(Expression<Func<Comercio, bool>> predicate, string[] includes, string caller = "")
        {
            switch (caller)
            {
                case "SearchPagedAsync":
                    if (RequestContext.CacheRequest.TryGetValue("tipoComercioId", out object tipoComercioId) &&  tipoComercioId != null)
                    {
                        int id = (int)tipoComercioId;
                        predicate = predicate.And(c => c.TipoComercio.Key == id);
                    }

                    if (RequestContext.CacheRequest.TryGetValue("userId", out object userId))
                    {
                        int id = (int)userId;
                        predicate = predicate.And(c => c.Cidadao.Key == id);
                    }
                    break;
            }
            return base.Where(predicate, includes, caller);
        }
    }
}
