using CidConnectada.Entities.Model.Enums;
using CidConnectada.Entities.Model.Notificacao;
using CidConnectada.Webapi.Models.Noticias;
using CidConnectada.Webapi.Models.Notificacao;
using System;
using Zenite.Pi.Context;

namespace CidConnectada.Webapi.Code.Map.Notificacao
{
    // (Entity => Dto)
    public class NotificacaoEntityToDtoMapperProfile : EntityToDtoBaseProfile
    {
        public NotificacaoEntityToDtoMapperProfile(Func<ContextRequest<int, string>> contextFactory)
            : base(contextFactory)
        {
            CreateMap<Notification, NotificationBaseDto>()
                .ForMember(dest => dest.prioridadeEnumNome, opt => opt.MapFrom(src => src.PrioridadeEnum))
                .ForMember(dest => dest.statusEnumNome, opt => opt.MapFrom(src => src.StatusEnum))
                .ForMember(dest => dest.destinoEnumNome, opt => opt.MapFrom(src => src.DestinoEnum))
                .AfterMap((src, dest) =>
                {
                    switch (src)
                    {
                        case NotificationUnicast unicast:
                            dest.tipoEnum = NotificationTypeEnum.Unicast;
                            dest.tipoEnumNome = nameof(NotificationTypeEnum.Unicast);
                            break;
                        case NotificationBroadcast broadcast:
                            dest.tipoEnum = NotificationTypeEnum.Broadcast;
                            dest.tipoEnumNome = nameof(NotificationTypeEnum.Broadcast);
                            break;
                        case NotificationMulticast multicast:
                            dest.tipoEnum = NotificationTypeEnum.Multicast;
                            dest.tipoEnumNome = nameof(NotificationTypeEnum.Multicast);
                            break;
                    }
                });

            CreateMap<Notification, NotificationDto>()
                .IncludeBase<Notification, NotificationBaseDto>();


            #region Unicast
            
            CreateMap<NotificationUnicast, NotificationBaseDto>()
                .IncludeBase<Notification, NotificationBaseDto>();

            CreateMap<NotificationUnicast, NotificationUnicastDto>()
                .IncludeBase<Notification, NotificationDto>()
                .ForMember(dest => dest.usuarioId, opt => opt.MapFrom(src => src.Usuario.Key))
                .ForMember(dest => dest.usuarioNome, opt => opt.MapFrom(src => src.Usuario.NomeCompleto));
            
            #endregion
            
            #region Multicast
            
            CreateMap<NotificationMulticast, NotificationBaseDto>()
                .IncludeBase<Notification, NotificationBaseDto>();
            
            #endregion

            #region Broadcast
            
            CreateMap<NotificationBroadcast, NotificationBaseDto>()
                .IncludeBase<Notification, NotificationBaseDto>();
            
            CreateMap<NotificationBroadcast, NotificationBroadcastDto>()
                .IncludeBase<Notification, NotificationDto>();
            
            #endregion
        }
    }
}