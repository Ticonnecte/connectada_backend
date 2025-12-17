using CidConnectada.Entities.Model.Relacionamento;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;
using Zenite.Pi.Entities.Model.Search;
using Zenite.Pi.Util.Control;
using Zenite.Pi.Util.Pagination;

namespace CidConnectada.Dao.Relacionamento
{
    public class HistoricoDialogoDao: BaseDao<HistoricoDialogo, HistoricoDialogoKey, int, string>
    {
        public HistoricoDialogoDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
        public override Dictionary<string, ListSortDirection> DefaultOrder => new Dictionary<string, ListSortDirection>
        {
            { "Dialogo.DhCriacao", ListSortDirection.Ascending }
        };

        public override string[] DefaultIncludes => new string[1]
        {
            "Funcionario"
        };

        public override IQueryable<HistoricoDialogo> Where(Expression<Func<HistoricoDialogo, bool>> predicate, string[] includes, string caller = "")
        {
            if (caller == "SearchPagedAsync")
            {
                if (RequestContext.CacheRequest.TryGetValue("DialogoKey", out object keyObject))
                {
                    predicate = predicate.And(h => h.Dialogo.Key == (string) keyObject);
                }
            }
            return base.Where(predicate, includes, caller);
        }
    }
}
