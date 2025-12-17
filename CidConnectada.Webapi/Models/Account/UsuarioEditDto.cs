using System.ComponentModel.DataAnnotations;

namespace CidConnectada.Webapi.Models.Account
{
    public class UsuarioEditDto : UsuarioDto
    {
        [Required]
        public override int key { get; set; }
    }
}