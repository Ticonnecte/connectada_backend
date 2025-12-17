using CidConnectada.Entities.Model.Notificacao;
using System;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Notificacao
{
    public class ExpoNotificationTokenDao : BaseDao<ExpoNotificationToken, int, int, string>
    {
        public ExpoNotificationTokenDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
        public override string[] DefaultIncludes
        {
            get => new string[2]
            {
                "User", "Device"
            };
        }
    }
}