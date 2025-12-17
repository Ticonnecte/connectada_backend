using System;
using System.ComponentModel.DataAnnotations;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Relacionamento
{
    public class ChangeDateDto : BaseEntityModel<string>
    {
        [Required]
        public DateTime data { get; set; }
    }
}