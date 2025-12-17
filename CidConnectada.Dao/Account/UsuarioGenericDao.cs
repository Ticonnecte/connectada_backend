using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using CidConnectada.Entities.Model.Account;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;
using static System.Int32;

namespace CidConnectada.Dao.Account
{
    public abstract class UsuarioGenericDao<TEntity> : MultiTenancyDao<TEntity, int, int, int, string>
        where TEntity : Usuario
    {
        public UsuarioGenericDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }

        public override string[] DefaultIncludes => ConcatanateIncludes(base.DefaultIncludes,
            new string[4]
            {
                "AspNetUsers.AspNetUserRolesSet.AspNetRoles", "RefreshTokenSet.Device", "Prefeitura", "VerificacaoConta"
            });

        protected override Expression<Func<TEntity, bool>> TenantPredicate
        {
            get
            {
                return e => e.TenantKey == TenantValue;
            }
        }

        protected override int TenantValue
        {
            get => 
                DaoHelper.GetTenantId() ?? base.TenantValue;
        }

        public TEntity FindByUsername(string userName, string[] includes = null)
        {
            return SingleOrDefault(u => u.AspNetUsers.Username == userName);
        }

        public TEntity ObterUser(string operationKey)
        {
            return SingleOrDefault(u => u.AspNetUsers.Key == operationKey);
        }
    }
}