using CidConnectada.Dao.Account;
using CidConnectada.Dao.Organograma;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Services.Impl.Identity;
using CidConnectada.Services.Intf.Account;
using CidConnectada.Services.Intf.Messaging;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Zenite.Pi.Context;
using Zenite.Pi.Exceptions;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Services.Impl.Account
{
    public class FuncionarioService : UsuarioGenericService<Funcionario, FuncionarioDao>, IFuncionarioService
    {
        public FuncionarioService(FuncionarioDao _cadDao,
            Func<ContextRequest<int, string>> contextFactory,
            AspNetUsersDao aspNetUsersDao,
            AspNetRolesDao aspNetRolesDao,
            RefreshTokenDao refreshTokenDao,
            DeviceDao deviceDao,
            PrefeituraDao prefeituraDao,
            VerificacaoContaDao verificacaoContaDao,
            IZApiService zApiService,
            Func<ApplicationUserManager> userManagerFactory
        ) : base(_cadDao,
            contextFactory,
            aspNetUsersDao,
            aspNetRolesDao,
            refreshTokenDao,
            deviceDao,
            prefeituraDao,
            verificacaoContaDao,
            zApiService,
            userManagerFactory)
        { }

        #region CRUD

        public override string GetNomeEntidade(int indexDetail = 0)
        {
            return "Funcionario";
        }

        public override object GetValorCampoDescritivoPadrao(Funcionario entity)
        {
            return base.GetValorCampoDescritivoPadrao(entity) + $" | Cpf: {entity.Cpf}";
        }

        #endregion
    }
}