using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Relacionamento;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;
using Zenite.Pi.Entities.Model.Search;
using Zenite.Pi.Util.Pagination;

namespace CidConnectada.Dao.Relacionamento
{
    public class DialogoDao : MultiTenancyDao<Dialogo, string, int, int, string>
    {
        public DialogoDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }

        public override Dictionary<string, ListSortDirection> DefaultOrder => new Dictionary<string, ListSortDirection>
        {
            { "DhCriacao", ListSortDirection.Descending }
        };

        public override string[] DefaultIncludes => new string[4]
        {
            "Secretaria", "Endereco", "HistoricoDialogoSet", "Cidadao"
        };
        
        protected override IQueryable<Dialogo> EntitySet
        {
            get
            {
                if (RequestContext.User is Cidadao)
                    return base.EntitySet.Where(e => e.Cidadao.AspNetUsers.Key == RequestContext.UserOperationKey);

                return base.EntitySet;
            }
        }

        protected override int TenantValue
        {
            get => DaoHelper.GetTenantId() ?? base.TenantValue;
        }
    }
}