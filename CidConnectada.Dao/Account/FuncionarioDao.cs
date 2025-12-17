using CidConnectada.Entities.Model.Account;
using System;
using Zenite.Pi.Context;

namespace CidConnectada.Dao.Account
{
    public class FuncionarioDao : UsuarioGenericDao<Funcionario>
    {
        public FuncionarioDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
    }
}