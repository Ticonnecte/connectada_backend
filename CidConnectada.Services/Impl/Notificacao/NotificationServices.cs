using CidConnectada.Dao.Notificacao;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Entities.Model.Notificacao;
using CidConnectada.Services.Intf.Account;
using CidConnectada.Services.Intf.Messaging;
using CidConnectada.Services.Intf.Notificacao;
using Expo.Server.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Zenite.Pi.Context;
using Zenite.Pi.Exceptions;
using Zenite.Pi.IoC;
using Zenite.Pi.Services.Impl;
using Zenite.Pi.Util.Parallel;
using Zenite.Wa;
using Zenite.Wa.Models.Zapi.Message;

namespace CidConnectada.Services.Impl.Notificacao
{
    public abstract class NotificationGenericService<TEntity, TDao> : CadastroBaseService<TEntity, TDao, int, int, string>
        where TEntity : Notification
        where TDao : NotificationGenericDao<TEntity>
    {
        public NotificationGenericService(
            TDao cadDao,
            Func<ContextRequest<int, string>> contextFactory,
            ExpoNotificationTokenDao expoNotificationTokenDao,
            NotificationUnicastDao notificationUnicastDao,
            NotificationMulticastDao notificationMulticastDao,
            IPushApiService pushApiService,
            IUsuarioService usuarioService,
            IZApiService zApiService
            )
          : base(cadDao, contextFactory)
        {
            ExpoNotificationTokenDao = expoNotificationTokenDao;
            NotificationUnicastDao = notificationUnicastDao;
            NotificationMulticastDao = notificationMulticastDao;
            PushApiService = pushApiService;
            UsuarioService = usuarioService;
            ZApiService = zApiService;
        }

        #region Daos-Services

        protected readonly ExpoNotificationTokenDao ExpoNotificationTokenDao;
        protected readonly NotificationUnicastDao NotificationUnicastDao;
        protected readonly NotificationMulticastDao NotificationMulticastDao;
        protected readonly IPushApiService PushApiService;
        protected readonly IUsuarioService UsuarioService;
        protected readonly IZApiService ZApiService;

        #endregion

        #region CRUD

        public override string GetNomeEntidade(int indexDetail = 0)
        {
            return "Notificação";
        }

        public override object GetValorCampoDescritivoPadrao(TEntity entity)
        {
            return $"Titulo: {entity.Title}";
        }

        protected override Expression<Func<TEntity, bool>> GetUnicidadeFilter(TEntity entity)
        {
            return e => e.Title == entity.Title && e.SubTitle == entity.SubTitle && e.Key != entity.Key;
        }

        #endregion

        #region Custom

        public override async Task<bool> CanDeleteAsync(TEntity entity)
        {
            bool result = entity.StatusEnum == NotificationStatusEnum.Pending;
            if (result)
            {
                return result;
            }
            else
            {
                throw new PiBusinessException("Operação abortada. Notificação não pode ser deletada pois já foi enviada.");
            }
        }

        public async Task<TEntity> Replace(int id, TEntity entity)
        {
            await ExcluirAsync(id);
            return await IncluirAsync(entity);
        }

        public async Task<ExpoNotificationToken> GetExpoToken(int userId, Guid deviceId)
        {
            return await ExpoNotificationTokenDao.FirstOrDefaultAsync(t => t.User.Key == userId && t.Device.Key == deviceId);
        }

        public ExpoNotificationToken InsertExpoToken(ExpoNotificationToken token)
        {
            if (token.Key == 0)
            {
                token.CreatedAt = DateTime.Now;
                token = ExpoNotificationTokenDao.Add(token);
            }
            else
            {
                token.UpdatedAt = DateTime.Now;
            }

            return token;
        }

        public async Task<TEntity> Send(int id)
        {
            TEntity entity = await ObterAsync(id);

            switch (entity)
            {
                case NotificationMulticast multicast:
                    // NotificationMulticastDao.Where(n => n.Key == entity.Key)
                    //     .Select(n => n.NotificationMulticastUserSet).Include(nmuSet => nmuSet.Select(nmu => nmu.Usuario)).ToHashSet();
                    break;
                case NotificationUnicast unicast:
                    await NotificationUnicastDao.Where(n => n.Key == entity.Key)
                        .Select(n => n.Usuario).FirstOrDefaultAsync();
                    break;
            }

            switch (entity.DestinoEnum)
            {
                case NotificationDestinyEnum.Push:
                    await SendPushNotification(entity);
                    break;

                case NotificationDestinyEnum.WhatsApp:
                    await SendWhatsAppNotification(entity);
                    break;

                case NotificationDestinyEnum.Both:
                    await SendPushNotification(entity);
                    await SendWhatsAppNotification(entity);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return entity;
        }

        private async Task SendWhatsAppNotification(TEntity entity)
        {
            if (ApplicationContext.AppSettings["Environment"] == nameof(ApiEnvironmentEnum.Development) && entity.GetType() == typeof(NotificationBroadcast))
                return;

            IList<Usuario> usuarioList = new List<Usuario>();

            switch (entity)
            {
                case NotificationBroadcast broadcast:
                    usuarioList = await UsuarioService.GetWhatsAppEnabledAsync();
                    break;

                case NotificationMulticast multicast:
                    IList<int> userIds = multicast.NotificationMulticastUserSet.Select(nmu => nmu.Usuario.Key).ToList();
                    usuarioList = await UsuarioService.GetManyAsync(userIds.ToArray(), new string[1] { "AspNetUsers" });
                    usuarioList = usuarioList.Where(u => u.AceitaMsgWhastApp).ToList();
                    break;

                case NotificationUnicast unicast:
                    int userId = unicast.Usuario.Key;
                    var usuario = await UsuarioService.ObterAsync(userId);

                    if (usuario.AceitaMsgWhastApp)
                        usuarioList.Add(usuario);
                    break;
            }

            HttpContext httpCxt = HttpContext.Current;

            short ParallelismCoefficient = 4;
            await usuarioList.ParallelForEachAsync(ParallelismCoefficient <= 0 ? 4 : ParallelismCoefficient, async usuario =>
            {
                if (HttpContext.Current == null)
                {
                    HttpContext.Current = httpCxt;
                }

                string phone = WhatsAppUtil.GetPhoneCleanUp(usuario.AspNetUsers.PhoneNumber);
                string message = entity.Title is null ? "" : $"*{entity.Title}*\n\n";
                message += entity.SubTitle is null ? "" : $"{entity.SubTitle}\n\n";
                message += $"{entity.Body}";
                message = message.Replace("~{Usuário.Nome}", usuario.NomeCompleto).Replace("~{Usuario.Nome}", usuario.Nome).Replace("~{Nome}", usuario.NomeCompleto);

                ZApiSendTextDto zApi = new ZApiSendTextDto
                {
                    phone = phone,
                    message = message
                };

                bool exists = true;

                try
                {
                    ZApiMsgResultDto response = await ZApiService.SendMessageAsync(zApi);
                }
                catch (Exception)
                {
                    log.Info($"Erro ao enviar notificação para o Usuário Id: '{usuario.Key}', Prefeitura Id: '{usuario.Prefeitura.Key}'");
                }
            });
        }

        private async Task SendPushNotification(TEntity entity)
        {
            var tokens = new List<string>();

            switch (entity)
            {
                case NotificationBroadcast broadcast:
                    tokens = await ExpoNotificationTokenDao.Where(e => true).Select(t => t.Token).ToListAsync();
                    break;

                case NotificationMulticast multicast:
                    IList<int> userIds = multicast.NotificationMulticastUserSet.Select(nmu => nmu.Usuario.Key).ToList();
                    tokens = await ExpoNotificationTokenDao.Where(e => userIds.Contains(e.User.Key)).Select(t => t.Token).ToListAsync();
                    break;

                case NotificationUnicast unicast:
                    int userId = unicast.Usuario.Key;
                    IList<string> userTokens = await ExpoNotificationTokenDao.Where(e => e.User.Key == userId)
                        .Select(e => e.Token).ToListAsync();

                    foreach (var token in userTokens)
                        tokens.Add(token);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!tokens.Any()) return;

            // Expo permite no máximo 100 tokens por vez
            const int batchSize = 100;

            for (int i = 0; i < tokens.Count; i += batchSize)
            {
                var batch = tokens.Skip(i).Take(batchSize).ToList();

                var pushTicketReq = new PushTicketRequest
                {
                    PushTo = batch,
                    PushTitle = entity.Title,
                    PushSubTitle = entity.SubTitle,
                    PushBody = entity.Body,
                    PushPriority = entity.PrioridadeEnum.ToString()
                };

                try
                {
                    PushTicketResponse result = await PushApiService.PushSendAsync(pushTicketReq);

                    if (result?.PushTicketErrors?.Any() == true)
                    {
                        foreach (PushTicketErrors error in result.PushTicketErrors)
                        {
                            log.Error($"Erro ao enviar push: {error.ErrorCode} - {error.ErrorMessage}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error($"Erro inesperado ao enviar push: {ex.Message}");
                }
                Thread.Sleep(900);
            }
        }

        #endregion

    }

    public class NotificationService : NotificationGenericService<Notification, NotificationDao>, INotificationService
    {
        public NotificationService(NotificationDao cadDao,
           Func<ContextRequest<int, string>> contextFactory,
           ExpoNotificationTokenDao expoNotificationTokenDao,
           NotificationUnicastDao notificationUnicastDao,
           NotificationMulticastDao notificationMulticastDao,
           IPushApiService pushApiService,
           IUsuarioService usuarioService,
           IZApiService zApiService

           )
            : base(cadDao, contextFactory,
                  expoNotificationTokenDao,
                  notificationUnicastDao,
                  notificationMulticastDao,
                  pushApiService,
                  usuarioService,
                  zApiService)
        {

        }

        // private INotificationBroadcastService BroadcastService { get => GetService<INotificationBroadcastService>(); }
        // private INotificationMulticastService MulticastService { get => GetService<INotificationMulticastService>(); }
        // private INotificationUnicastService UnicastService { get => GetService<INotificationUnicastService>(); }
        //
        // public Task Send(int id)
        // {
        //     throw new NotImplementedException();
        // }
    }

    public class NotificationBroadcastService : NotificationGenericService<NotificationBroadcast, NotificationBroadcastDao>, INotificationBroadcastService
    {
        public NotificationBroadcastService(NotificationBroadcastDao cadDao, Func<ContextRequest<int, string>> contextFactory,
            ExpoNotificationTokenDao expoNotificationTokenDao,
            NotificationUnicastDao notificationUnicastDao,
            NotificationMulticastDao notificationMulticastDao,
            IPushApiService pushApiService,
            IUsuarioService usuarioService,
            IZApiService zApiService
            )
            : base(cadDao, contextFactory,
                   expoNotificationTokenDao,
                  notificationUnicastDao,
                  notificationMulticastDao,
                  pushApiService,
                  usuarioService,
                  zApiService)
        {

        }
    }

    public class NotificationMulticastService : NotificationGenericService<NotificationMulticast, NotificationMulticastDao>, INotificationMulticastService
    {
        public NotificationMulticastService(NotificationMulticastDao cadDao, Func<ContextRequest<int, string>> contextFactory,
            ExpoNotificationTokenDao expoNotificationTokenDao,
            NotificationUnicastDao notificationUnicastDao,
            NotificationMulticastDao notificationMulticastDao,
            IPushApiService pushApiService,
            IUsuarioService usuarioService,
            IZApiService zApiService
            )
            : base(cadDao, contextFactory,
                  expoNotificationTokenDao,
                  notificationUnicastDao,
                  notificationMulticastDao,
                  pushApiService,
                  usuarioService,
                  zApiService)
        {
        }
    }

    public class NotificationUnicastService : NotificationGenericService<NotificationUnicast, NotificationUnicastDao>, INotificationUnicastService
    {
        public NotificationUnicastService(NotificationUnicastDao cadDao, Func<ContextRequest<int, string>> contextFactory,
            ExpoNotificationTokenDao expoNotificationTokenDao,
            NotificationUnicastDao notificationUnicastDao,
            NotificationMulticastDao notificationMulticastDao,
            IPushApiService pushApiService,
            IUsuarioService usuarioService,
            IZApiService zApiService
            )
            : base(cadDao, contextFactory,
                   expoNotificationTokenDao,
                  notificationUnicastDao,
                  notificationMulticastDao,
                  pushApiService,
                  usuarioService,
                  zApiService)
        {
        }
    }
}
