using CidConnectada.Entities.Model.Comercios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Dao.Comercios
{
    public class ProdutoDao: BaseDao<Produto, string, int, string>
    {
        public ProdutoDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
        public override string[] DefaultIncludes => new string[1] { "Comercio.Cidadao" };

        public override IQueryable<Produto> Where(Expression<Func<Produto, bool>> predicate, string[] includes, string caller = "")
        {
            switch (caller)
            {
                case "SearchPagedAsync":
                    if (RequestContext.CacheRequest.TryGetValue("comercioId", out object comercioId) && comercioId != null)
                    {
                        string id = (string)comercioId;
                        predicate = predicate.And(p => p.Comercio.Key == id);
                    }
                    break;
            }
            return base.Where(predicate, includes, caller);
        }

    }
}
