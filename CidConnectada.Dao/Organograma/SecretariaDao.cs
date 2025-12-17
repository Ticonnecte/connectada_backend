using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CidConnectada.Entities.Model.Organograma;
using CidConnectada.Webapi.Models.Organograma;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Organograma
{
    public class SecretariaDao : MultiTenancyDao<Secretaria, string, int, int, string>
    {
        public SecretariaDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }

        public override Dictionary<string, ListSortDirection> DefaultOrder
        {
            get => new Dictionary<string, ListSortDirection>
            {
                { "OrdemHome", ListSortDirection.Ascending },
                { "Nome", ListSortDirection.Ascending }
            };
        }

        public override string[] DefaultIncludes
        {
            get => new string[2] { "Prefeitura", "SecretariaMenuSet.RotaInterna" };
        }
        
        protected override int TenantValue
        {
            get => DaoHelper.GetTenantId() ?? base.TenantValue;
        }

        public async Task AlterarOrdemHome(IList<OrdemHomeDto<string>> ordemList)
        {
            var inputTable = new DataTable();
            inputTable.Columns.Add("ID", typeof(string));
            inputTable.Columns.Add("ORDEM", typeof(byte));

            foreach (var item in ordemList)
                inputTable.Rows.Add(item.key, item.ordemHome);

            IList<SqlParameter> parametros = new List<SqlParameter>();

            string sql = "EXECUTE dbo.ALTERAR_ORDEM_SECRETARIA @ORDEM;";

            parametros.Add(new SqlParameter("@ORDEM", SqlDbType.Structured)
            {
                TypeName = "dbo.ORDEM_HOME_TVP",
                Direction = ParameterDirection.Input,
                Value = inputTable
            });

            await Context.Database.ExecuteSqlCommandAsync(sql, parametros.ToArray());
        }
    }
}