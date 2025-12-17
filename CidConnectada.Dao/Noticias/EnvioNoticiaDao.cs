using CidConnectada.Entities.Model.Noticias;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Noticias
{
    public class EnvioNoticiaDao : BaseDao<EnvioNoticia, EnvioNoticiaKey, int, string>
    {
        public EnvioNoticiaDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
        public override Dictionary<string, ListSortDirection> DefaultOrder => new Dictionary<string, ListSortDirection>
        {
            {
                "DhEnvio", ListSortDirection.Ascending
            }
        };

        public override string[] DefaultIncludes => new string[2]
        {
            "Noticia", "Usuario"
        };

        // public async Task<int> SendedMessageRegister(EnvioNoticia entity)
        // {
        //     SqlDatabase sqlDb = new SqlDatabase(Context.Database.Connection.ConnectionString);
        //     List<DbParameter> paramList = new List<DbParameter>(2);
        //
        //     SqlParameter param = new SqlParameter("@NOTICIA_ID", SqlDbType.VarChar)
        //     {
        //         Direction = ParameterDirection.Input,
        //         Size = 16,
        //         Value = entity.NoticiaId
        //     };
        //     paramList.Add(param);
        //     param = new SqlParameter("@USUARIO_ID", SqlDbType.VarChar)
        //     {
        //         Direction = ParameterDirection.Input,
        //         Size = 100,
        //         Value = entity.ZaapId
        //     };
        //     paramList.Add(param);
        //     param = new SqlParameter("@ZAAP_ID", SqlDbType.VarChar)
        //     {
        //         Direction = ParameterDirection.Input,
        //         Size = 100,
        //         Value = entity.ZaapId
        //     };
        //     paramList.Add(param);
        //     param = new SqlParameter("@STATUS_ENUM", SqlDbType.Int)
        //     {
        //         Direction = ParameterDirection.Input,
        //         Value = entity.StatusEnum
        //     };
        //     paramList.Add(param);
        //     return await sqlDb.ExecuteStoredProcedureAsync("SENDED_MSG_REGISTER", paramList);
        // }
    }
}