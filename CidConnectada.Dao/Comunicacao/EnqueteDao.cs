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
    public class EnqueteDao : MultiTenancyDao<Enquete, int, int, int, string>
    {
        public EnqueteDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
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

        public override string[] DefaultIncludes { get => new string[1] { "EnqueteOpcaoSet" }; }

        public override IQueryable<Enquete> Where(Expression<Func<Enquete, bool>> predicate, string[] includes, string caller = "")
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