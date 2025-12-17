using System;
using AutoMapper;
using CidConnectada.Entities.Model.Account;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Noticias
{
    public class NoticiaLog : BaseEntity<NoticiaLogKey>, IEquatable<NoticiaLog>
    {
        [IgnoreMap]
        public override NoticiaLogKey Key => new NoticiaLogKey
        {
            NoticiaId = NoticiaId,
            DhUpdate = DhUpdate
        };

        public string NoticiaId { get; set; }
        public DateTime DhUpdate { get; set; }

        public Noticia Noticia { get; set; }
        public Usuario Usuario { get; set; }

        public bool Equals(NoticiaLog other)
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