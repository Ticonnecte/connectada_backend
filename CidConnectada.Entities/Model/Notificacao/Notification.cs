using System;
using System.Collections.Generic;
using CidConnectada.Entities.Model.Enums;
using Zenite.Pi.Entities.Model.MultiTenancy;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Notificacao
{
    public class Notification : MultiTenancy<int, int>, IEquatable<Notification>
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Body { get; set; }
        public string DataJson { get; set; }
        public NotificationDestinyEnum DestinoEnum { get; set; }
        public NotificationPriorityEnum? PrioridadeEnum { get; set; }
        public NotificationStatusEnum StatusEnum { get; set; }
        public DateTime? DhAgendamento { get; set; }

        public ISet<ExpoNotificationStatus> ExpoNotificationStatusSet { get; set; } = new HashSet<ExpoNotificationStatus>();
        public ISet<WaNotificacaoStatus> WaNotificacaoStatusSet { get; set; } = new HashSet<WaNotificacaoStatus>();

        public bool Equals(Notification other)
        {
            bool result;
            if (ReferenceEquals(other, null))
                result = false;
            else if (ReferenceEquals(other, this))
                result = true;
            else
                result = EntityUtil.EqualsEntity(this, other);

            return result;
        }
    }
}