using System;
using AutoMapper;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Enums;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Noticias
{
    public class EnvioNoticia : BaseEntity<EnvioNoticiaKey>, IEquatable<EnvioNoticia>
    {
        [IgnoreMap]
        public override EnvioNoticiaKey Key => new EnvioNoticiaKey
        {
            NoticiaId = NoticiaId,
            UsuarioId = UsuarioId
        };

        public string NoticiaId { get; set; }
        public int UsuarioId { get; set; }

        public DateTime? DhEnvio { get; set; }
        public DateTime? DeliveryEvent { get; set; }
        public DateTime? ReceivedEvent { get; set; }
        public DateTime? ReadEvent { get; set; }
        public Noticia Noticia { get; set; }
        public Usuario Usuario { get; set; }

        public EnvioMsgStatusEnum StatusEnum { get; set; }
        public string ZaapId { get; set; }
        public string MessageId { get; set; }

        public bool Equals(EnvioNoticia other)
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