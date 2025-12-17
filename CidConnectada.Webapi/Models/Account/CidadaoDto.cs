using System.ComponentModel.DataAnnotations;

namespace CidConnectada.Webapi.Models.Account
{
    public class CidadaoDto : UsuarioDto
    {
        public int bairroId { get; set; }

        [Required]
        public override string telefone
        {
            get => _telefone;
            set => _telefone = RemoverCaracteresNaoNumericos(value);
        }
    }
}