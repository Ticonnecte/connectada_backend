using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Organograma;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Zenite.Pi.Entities.Model.MultiTenancy;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Noticias
{
    public class Noticia : HtmlContent, IEquatable<Noticia>
    {
        public override string Key { get; set; }
        public bool EnviarWhatsApp { get; set; }


        public Prefeitura Prefeitura { get; set; }
        public ISet<NoticiaCategoriaVinc> NoticiaCategoriaVincSet { get; set; } = new HashSet<NoticiaCategoriaVinc>();
        public ISet<NoticiaLog> NoticiaLogSet { get; set; } = new HashSet<NoticiaLog>();
        public ISet<EnvioNoticia> EnvioNoticiaSet { get; set; } = new HashSet<EnvioNoticia>();
        public ISet<NoticiaImages> NoticiaImagesSet { get; set; }


        public bool Equals(Noticia other)
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

        public override string UrlPart()
        {
          return "noticias";
        }
    }
}
