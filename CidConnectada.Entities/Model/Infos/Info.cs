using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Noticias;
using CidConnectada.Entities.Model.Organograma;
using CidConnectada.Entities.Model.Relacionamento;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Zenite.Pi.Entities;
using Zenite.Pi.Entities.Model.MultiTenancy;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Infos
{
    public class Info: HtmlContent, IEquatable<Info>//, IAuditable
    {
       
        // to do: subir para HtmlContent depois de adicionar em Notícia...

        [Required]
        public Categoria Categoria { get; set; }

        public ISet<InfoImages> InfoImagesSet { get; set; }

        //public string CreationUser { get; set; }
        //public string UpdateUser { get; set; }
        //public DateTime? UpdateDate { get; set; }
        //public DateTime CreationDate { get; set; }
        //public byte[] Version { get; set; }

        public bool Equals(Info other)
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
            return "informacoes";
        }
    }
}
