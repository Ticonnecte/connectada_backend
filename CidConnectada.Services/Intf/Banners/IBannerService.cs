using System.Collections.Generic;
using System.Threading.Tasks;
using CidConnectada.Entities.Model.Banners;
using CidConnectada.Services.Intf.AWS;
using Zenite.Pi.Services;

namespace CidConnectada.Services.Intf.Banners
{
    //public interface IBannerService : ICadastroService<Banner, string>
    //{
    //    Task<IList<RotaInterna>> GetRotasInternasAsync();
    //    RotaInterna FindRotaById(int linkId);
    //    Task<IList<Banner>> GetHomeBannersAsync();
    //}

    public interface IBannerService : IS3FileServiceGeneric<Banner>
    {
        Task<IList<RotaInterna>> GetRotasInternasAsync();
        RotaInterna FindRotaById(int linkId);
        Task<IList<Banner>> GetHomeBannersAsync();
    }
}