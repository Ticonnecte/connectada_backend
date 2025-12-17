using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Entities.Model.Notificacao;
using CidConnectada.Services.Intf.Account;
using CidConnectada.Webapi.Models.Noticias;
using CidConnectada.Webapi.Models.Notificacao;
using System;
using Zenite.Pi.Context;

namespace CidConnectada.Webapi.Code.Map.Notificacao
{
    public class NotificacaoDtoToEntityMapperProfile : DtoToEntityBaseProfile
    {
        #region Services
        protected IUsuarioService UsuarioService => GetService<IUsuarioService>();


        #endregion
        public NotificacaoDtoToEntityMapperProfile(Func<ContextRequest<int, string>> contextFactory
        )
            : base(contextFactory)
        {

            CreateMap<NotificationBaseDto, Notification>()
                .ForMember(dest => dest.StatusEnum, opt =>
                {
                    opt.PreCondition(src => src.isNew);
                    opt.MapFrom(src => NotificationStatusEnum.Pending);
                });

            CreateMap<NotificationDto, Notification>()
                .IncludeBase<NotificationBaseDto, Notification>();

            CreateMap<NotificationUnicastDto, NotificationUnicast>()
                .IncludeBase<NotificationDto, Notification>()
                .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => UsuarioService.Obter(src.usuarioId, null)));

            CreateMap<NotificationBroadcastDto, NotificationBroadcast>()
                .IncludeBase<NotificationDto, Notification>();

            CreateMap<InsertExpoTokenDto, ExpoNotificationToken>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => UsuarioService.Obter(((Usuario)Context.User).Key, null)))
                .ForMember(dest => dest.Device, opt => opt.MapFrom(src => UsuarioService.FindDevice(src.deviceId)));
        }

    }
}