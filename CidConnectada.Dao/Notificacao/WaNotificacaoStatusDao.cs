using CidConnectada.Entities.Model.Notificacao;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Notificacao
{
    public class WaNotificacaoStatusDao : BaseDao<WaNotificacaoStatus, NotificationUserKey, int, string>
    {
        public WaNotificacaoStatusDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
        public override Dictionary<string, ListSortDirection> DefaultOrder
        {
            get => new Dictionary<string, ListSortDirection>
            {
                {
                    "SentAt", ListSortDirection.Ascending
                }
            };
        }

        public override string[] DefaultIncludes
        {
            get => new string[2]
            {
                "Notification", "Usuario"
            };
        }
    }
}