using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Entities.Model.Notificacao;
using CidConnectada.Services.Intf.Noticias;
using CidConnectada.Services.Intf.Notificacao;
using CidConnectada.Webapi.Models.Noticias;
using CidConnectada.Webapi.Models.Notificacao;
using CidConnectada.Website.Filters;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Zenite.Pi.Context;
using Zenite.Pi.Entities.Enums;
using Zenite.Pi.Entities.Model.Search;
using Zenite.Pi.Exceptions;
using Zenite.Pi.Web.Models.Cadastro;
using Zenite.Pi.Web.Models.Pesquisa;
using Zenite.Pi.Web.WebApi;
using Zenite.Wa.Models.Zapi.Message;

namespace CidConnectada.Webapi.Controllers.Notificacao
{
    public class NotificationGenericController<TEntity, TDto, TService> : BaseWebApiController<TEntity, TDto, TService, int, int, string>
        where TEntity : Notification
        where TDto : NotificationDto
        where TService : INotificationGenericService<TEntity>
    {

        public NotificationGenericController(
            TService cadService, 
            AutoMapper.IMapper mapper,
            Func<ContextRequest<int, string>> contextFactory,
            INotificationService notificationService
        )
            : base(cadService, mapper, contextFactory)
        {
            NotificationService = notificationService;
            GeneroEntidade = GenreEnum.Male;
            Title = "Notificação";
        }

        private readonly INotificationService NotificationService;

        public override async Task<IHttpActionResult> Put(TDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            try
            {
                Notification entity = await NotificationService.ObterAsync(model.key, null);
                
                switch (entity)
                {
                    case NotificationBroadcast broadcast:
                        if (!(model is NotificationBroadcastDto))
                        {
                            if (entity.StatusEnum > NotificationStatusEnum.Pending)
                                return BadRequest("Não é possível alterar o tipo de uma notificação após ter sido enviada.");
                            
                            model.isNew = true;
                            TEntity newEntity = AMapper.Map<TEntity>(model);
                            newEntity.StatusEnum = entity.StatusEnum;
                            await cadService.Replace(model.key, newEntity);
                            return Ok(GetPutResult<TDto>(newEntity));
                        }
                        break;
                    
                    // case NotificationMulticast multicast:
                    //     if (!(model is NotificationMulticastDto))
                    //     {
                    //         if (entity.StatusEnum > NotificationStatusEnum.Pending)
                    //              return BadRequest("Não é possível alterar o tipo de uma notificação após ter sido enviada.");
                    //
                    //         model.isNew = true;
                    //         TEntity newEntity = AMapper.Map<TEntity>(model);
                    //         newEntity.StatusEnum = entity.StatusEnum;
                    //         await cadService.Replace(model.key, newEntity);
                    //         return Ok(GetPutResult<TDto>(newEntity));
                    //     }
                    //     break;
                    
                    case NotificationUnicast unicast:
                        if (!(model is NotificationUnicastDto))
                        {
                            if (entity.StatusEnum > NotificationStatusEnum.Pending)
                                return BadRequest("Não é possível alterar o tipo de uma notificação após ter sido enviada.");
                            
                            model.isNew = true;
                            TEntity newEntity = AMapper.Map<TEntity>(model);
                            newEntity.StatusEnum = entity.StatusEnum;
                            await cadService.Replace(model.key, newEntity);
                            return Ok(GetPutResult<TDto>(newEntity));
                        }
                        break;
                }
                return await base.Put(model);
            }
            catch (PiBusinessException e)
            {
                return BadRequest(e.Message);
            }
        }

        #region WhatsApp

        public async Task<IHttpActionResult> DeliveryWhatsApp(ZaapDeliveryHookDto model)
        {
            log.Info(string.Format("Confirmação de entrega de mensagem para o número '{0}'", model.phone));
            //await cadService.SetDelivery(model.zaapId, model.messageId, model.type.Replace("Callback", ""), model.momment);
            return Ok(new { success = true });
        }

        #endregion
    }

    [RoutePrefix("api/Notificacao")]
    [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO,CIDADAO")]
    public class NotificationController : NotificationGenericController<Notification, NotificationDto, INotificationService>
    {
        public NotificationController(
            INotificationService cadService,
            AutoMapper.IMapper mapper,
            Func<ContextRequest<int, string>> contextFactory
        )
            : base(cadService, mapper, contextFactory, cadService)
        {
            GeneroEntidade = GenreEnum.Female;
            Title = "NoticiaCategoria";
        }

        #region Custom

        [HttpPost]
        [Route("InsertExpoToken")]
        public async Task<IHttpActionResult> InsertExpoToken(InsertExpoTokenDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int userId = ((Usuario)Context.User).Key;
            ExpoNotificationToken entity = await cadService.GetExpoToken(userId, model.deviceId);

            if (entity is null)
            {
                entity = AMapper.Map<ExpoNotificationToken>(model);
            }
            else
            {
                entity.Token = model.token;
            }

            cadService.InsertExpoToken(entity);
            return Ok("Token inserido com sucesso.");
        }

        [HttpPost]
        [Route("Send")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public async Task<IHttpActionResult> Send(int id)
        {
            Notification entity = await cadService.Send(id);
            entity.StatusEnum = NotificationStatusEnum.Sent;
            await cadService.AlterarAsync(entity);

            return Ok("Notificação enviada com sucesso.");
        }

        #endregion

        #region CRUD

        [HttpDelete]
        [Route("Delete")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public override Task<IHttpActionResult> Delete(int id)
        {
            return base.Delete(id);
        }

        [HttpGet]
        [Route("GetFiltered")]
        [ResponseType(typeof(SearchResultDto<NotificationBaseDto>))]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public override async Task<IHttpActionResult> GetFiltered([FromUri] ContainsFilter filter)
        {
            return Ok(await base.GetFilteredGeneric<NotificationBaseDto>(filter));
        }

        #endregion

    }
    
    [RoutePrefix("api/NotificacaoBroadcast")]
    [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
    public class NotificationBroadcastController : NotificationGenericController<NotificationBroadcast, NotificationBroadcastDto, INotificationBroadcastService>
    {
        public NotificationBroadcastController(
            INotificationBroadcastService cadService,
            AutoMapper.IMapper mapper,
            Func<ContextRequest<int, string>> contextFactory,
            INotificationService notificationService
        )
            : base(cadService, mapper, contextFactory, notificationService)
        {
            GeneroEntidade = GenreEnum.Female;
            Title = "Notificação Broadcast";
        }

        #region CRUD

        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(NotificationBroadcastDto))]
        public override async Task<IHttpActionResult> Post(NotificationBroadcastDto dto)
        {
            return await base.Post(dto);
        }

        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(NotificationBroadcastDto))]
        public override async Task<IHttpActionResult> Put(NotificationBroadcastDto dto)
        {
            Context.CacheRequest.Add("Put", true);
            return await base.Put(dto);
        }
        
        [HttpGet]
        [Route("GetOne")]
        [ResponseType(typeof(NotificationBroadcastDto))]
        public override async Task<IHttpActionResult> GetOne(int id)
        {
            return await base.GetOne(id);
        }

        #endregion

    }
    
    [RoutePrefix("api/NotificacaoUnicast")]
    [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
    public class NotificationUnicastController : NotificationGenericController<NotificationUnicast, NotificationUnicastDto, INotificationUnicastService>
    {
        public NotificationUnicastController(
            INotificationUnicastService cadService,
            AutoMapper.IMapper mapper,
            Func<ContextRequest<int, string>> contextFactory,
            INotificationService notificationService
        )
            : base(cadService, mapper, contextFactory, notificationService)
        {
            GeneroEntidade = GenreEnum.Female;
            Title = "Notificação Unicast";
        }

        #region CRUD

        [HttpPost]
        [Route("Post")]
        [ResponseType(typeof(NotificationUnicastDto))]
        public override async Task<IHttpActionResult> Post(NotificationUnicastDto dto)
        {
            return await base.Post(dto);
        }

        [HttpPut]
        [Route("Put")]
        [ResponseType(typeof(NotificationUnicastDto))]
        public override async Task<IHttpActionResult> Put(NotificationUnicastDto dto)
        {
            Context.CacheRequest.Add("Put", true);
            return await base.Put(dto);
        }

        [HttpGet]
        [Route("GetOne")]
        [ResponseType(typeof(NotificationUnicastDto))]
        public override async Task<IHttpActionResult> GetOne(int id)
        {
            return await base.GetOne(id);
        }

        #endregion
        
    }
}

