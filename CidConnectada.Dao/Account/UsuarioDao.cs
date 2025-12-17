using CidConnectada.Entities.Model.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Routing;
using Zenite.Pi.Context;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Dao.Account
{
    public class UsuarioDao : UsuarioGenericDao<Usuario>
    {
        public UsuarioDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
        #region Custom

        public override IQueryable<Usuario> Where(Expression<Func<Usuario, bool>> predicate, string[] includes, string caller = "")
        {
            switch (caller)
            {
                case "SearchPagedAsync":
                    if (RequestContext.CacheRequest.TryGetValue("GetFilteredAdmin", out object _))
                    {
                        predicate = predicate.And(u => u.AspNetUsers.AspNetUserRolesSet.Any(ur => ur.AspNetRoles.Name == "ADMIN"));
                    }
                    break;
            }
            return base.Where(predicate, includes, caller);
        }

        protected override Expression<Func<Usuario, bool>> TenantPredicate
        {
            get
            {
                bool isSa = RequestContext.User != null && ((Usuario)RequestContext.User).AspNetUsers.AspNetUserRolesSet.Any(ua => ua.AspNetRoles.Name == "SA");
                return e => e.TenantKey == TenantValue || (e.TenantKey == 0 && isSa);
            }
        }

        protected override int TenantValue
        {
            get {
                bool isSa = RequestContext.User != null && ((Usuario)RequestContext.User).AspNetUsers.AspNetUserRolesSet.Any(ua => ua.AspNetRoles.Name == "SA");
                return isSa ? 0 : DaoHelper.GetTenantId() ?? base.TenantValue;
            }
        }

        public override Dictionary<string, ListSortDirection> DefaultOrder => new Dictionary<string, ListSortDirection>()
        {
            { "Nome", ListSortDirection.Ascending }
        };
        #endregion
    }
}