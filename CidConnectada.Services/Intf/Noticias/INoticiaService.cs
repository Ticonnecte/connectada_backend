using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Dto;
using CidConnectada.Entities.Model.Infos;
using CidConnectada.Entities.Model.Noticias;
using CidConnectada.Services.Impl.Noticias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Zenite.Pi.Services;

namespace CidConnectada.Services.Intf.Noticias
{
    public interface INoticiaService : ICadastroMasterService<Noticia, string, NoticiaImages, HtmlImagesKey>
    {
        [TransactionRequired]
        Task SendedMessageRegister(EnvioNoticia envioNoticia, string zaapId, string messageId);

        [TransactionRequired()]
        Task AlterarAsync(Noticia entity, ISet<NoticiaImages> listEntitiesDetail1, Delegate upload);

        [TransactionRequired()]
        Task DeleteAsync(Noticia entity, Delegate deleteS3);
      

        [TransactionRequired()]
        Task<Noticia> IncluirAsync(Noticia entity, Delegate upload);

    }
}
