using CidConnectada.Entities.Model.Account;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Account
{
    public class FuncionarioMap : EntityBaseMap<Funcionario, int>
    {
        public FuncionarioMap()
        {
            ToTable("FUNCIONARIO");
        }
    }
}