using System.ComponentModel.DataAnnotations;

namespace CidConnectada.Webapi.Models.Account
{
    public class FuncionarioDto : UsuarioDto
    {
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        public override string email { get; set; }
    }
}