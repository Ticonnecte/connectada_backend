using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Noticias
{
    public class EnviarNoticiaWppDto : BaseEntityModel<string>
    {
        public string mensagem { get; set; }
    }
}