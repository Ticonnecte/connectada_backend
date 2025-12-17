using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using CidConnectada.Entities.Model.Comunicacao;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Comunicacao
{
    public class PesquisaDao : MultiTenancyDao<Pesquisa, int, int, int, string>
    {
        public PesquisaDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }

        public override Dictionary<string, ListSortDirection> DefaultOrder
        {
            get => new Dictionary<string, ListSortDirection>
            {
                { "VigenciaInicio", ListSortDirection.Descending },
                { "Nome", ListSortDirection.Ascending }
            };
        }

        protected override int TenantValue
        {
            get => DaoHelper.GetTenantId() ?? base.TenantValue;
        }

        public override IQueryable<Pesquisa> Where(Expression<Func<Pesquisa, bool>> predicate, string[] includes, string caller = "")
        {
            var query = base.Where(predicate, includes, caller);

            switch (caller)
            {
                case "SearchPagedAsync":
                    if (RequestContext.CacheRequest.TryGetValue("GetVigentes", out object _))
                    {
                        query = query.Where(e => e.VigenciaInicio <= DateTime.Now && e.VigenciaFinal >= DateTime.Now);
                    }

                    break;
            }

            return query;
        }
    }
}