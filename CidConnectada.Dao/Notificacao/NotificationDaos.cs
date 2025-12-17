using System;
using System.Collections.Generic;
using System.ComponentModel;
using CidConnectada.Entities.Model.Notificacao;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Notificacao
{
    public abstract class NotificationGenericDao<TEntity> : MultiTenancyDao<TEntity, int, int, int, string>
        where TEntity : Notification
    {
        public NotificationGenericDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }

        public override Dictionary<string, ListSortDirection> DefaultOrder
        {
            get => new Dictionary<string, ListSortDirection>
            {
                {
                    "DhAgendamento", ListSortDirection.Ascending
                },
                {
                    "StatusEnum", ListSortDirection.Ascending
                }
            };
        }
        
        protected override int TenantValue
        {
            get => DaoHelper.GetTenantId() ?? base.TenantValue;
        }
    }

    public class NotificationDao : NotificationGenericDao<Notification>
    {
        public NotificationDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
    }

    public class NotificationBroadcastDao : NotificationGenericDao<NotificationBroadcast>
    {
        public NotificationBroadcastDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
    }

    public class NotificationMulticastDao : NotificationGenericDao<NotificationMulticast>
    {
        public NotificationMulticastDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }

        public override string[] DefaultIncludes
        {
            get => new string[1]
            {
                "NotificationMulticastUser.Usuario"
            };
        }
    }

    public class NotificationUnicastDao : NotificationGenericDao<NotificationUnicast>
    {
        public NotificationUnicastDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }

        public override string[] DefaultIncludes
        {
            get => new string[1]
            {
                "Usuario"
            };
        }
    }
}