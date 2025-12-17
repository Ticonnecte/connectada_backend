using System.ComponentModel.DataAnnotations;

namespace CidConnectada.Webapi.Models.Account
{
    public class CidadaoEditDto : UsuarioEditDto
    {
        public int bairroId { get; set; }
    }
}