using CidConnectada.Entities.Model.Infos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenite.Pi.Entities;

namespace CidConnectada.Entities.Model.AWS
{
    public class HtmlImages : BaseEntity<HtmlImagesKey>
    {
        public override HtmlImagesKey Key => new HtmlImagesKey
        {
            HashId = this.HashId,
            ParentId = this.ParentId
        };
        public HtmlImages()
        {
        }

        public HtmlImages(int hashId, string parentId)
        {
            HashId = hashId;
            parentId = ParentId;
        }

        public int HashId { get; set; }
        public string ParentId { get; set; }
        public string ImgUrl { get; set; }

        [NotMapped]
        public string Base64 { get; set; }

    }
}
