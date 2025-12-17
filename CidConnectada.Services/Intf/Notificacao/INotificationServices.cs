using System;
using System.Threading.Tasks;
using CidConnectada.Entities.Model.Notificacao;
using Zenite.Pi.Services;

namespace CidConnectada.Services.Intf.Notificacao
{
    public interface INotificationGenericService<TEntity> : ICadastroService<TEntity, int>
        where TEntity : Notification
    {
        [TransactionRequired]
        Task<TEntity> Replace(int id, TEntity entity);
        Task<ExpoNotificationToken> GetExpoToken(int userId, Guid deviceId);

        [TransactionRequired]
        ExpoNotificationToken InsertExpoToken(ExpoNotificationToken token);

        Task<TEntity> Send(int id);
    }

    public interface INotificationService : INotificationGenericService<Notification>
    {
    }

    public interface INotificationBroadcastService : INotificationGenericService<NotificationBroadcast>
    {
    }

    public interface INotificationMulticastService : INotificationGenericService<NotificationMulticast>
    {
    }

    public interface INotificationUnicastService : INotificationGenericService<NotificationUnicast>
    {
    }
}