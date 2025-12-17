using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Infos;
using CidConnectada.Services.Impl.Infos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Zenite.Pi.Services;

namespace CidConnectada.Services.Intf.Infos
{
    public interface IInfoService : ICadastroMasterService<Info, string, InfoImages, HtmlImagesKey>
    {
        [TransactionRequired]
        Task<Info> IncluirAsync(Info entity, Delegate upload);

        [TransactionRequired()]
        Task AlterarAsync(Info entity, ISet<InfoImages> listEntitiesDetail1, Delegate upload);

        [TransactionRequired()]
        Task DeleteAsync(Info entity, Delegate deleteS3);
        IList<Info> GetAtivasByCategoria(int categoriaKey);
    }
}
