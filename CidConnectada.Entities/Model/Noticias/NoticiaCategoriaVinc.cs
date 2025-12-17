using System;
using AutoMapper;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Noticias
{
    public class NoticiaCategoriaVinc : BaseEntity<NoticiaCategoriaVincKey>, IEquatable<NoticiaCategoriaVinc>
    {
        [IgnoreMap]
        public override NoticiaCategoriaVincKey Key => new NoticiaCategoriaVincKey
        {
            NoticiaId = NoticiaId,
            CategoriaId = CategoriaId
        };

        public string NoticiaId { get; set; }
        public int CategoriaId { get; set; }

        public Noticia Noticia { get; set; }
        public NoticiaCategoria NoticiaCategoria { get; set; }

        public bool Equals(NoticiaCategoriaVinc other)
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