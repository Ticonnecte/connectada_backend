using System;
using System.Collections.Generic;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Entities.Model.Organograma;
using Zenite.Pi.Entities.Model.MultiTenancy;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Noticias
{
    public class NoticiaCategoria : MultiTenancy<int, int>, IEquatable<NoticiaCategoria>
    {
        public string Nome { get; set; }
        public CorEnum Cor { get; set; }
        public string Descricao { get; set; }
        public string IconeNome { get; set; }

        public Prefeitura Prefeitura { get; set; }
        public ISet<NoticiaCategoriaVinc> NoticiaCategoriaVincSet => new HashSet<NoticiaCategoriaVinc>();

        public bool Equals(NoticiaCategoria other)
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