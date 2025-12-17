using CidConnectada.Entities.Model.Account;
using System;
using Zenite.Pi.Context;

namespace CidConnectada.Dao.Account
{
    public class CidadaoDao : UsuarioGenericDao<Cidadao>
    {
        public CidadaoDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
    }
}