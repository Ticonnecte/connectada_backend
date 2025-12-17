using System.Threading.Tasks;
using CidConnectada.Entities.Model.Emprego;
using Zenite.Pi.Services;

namespace CidConnectada.Services.Intf.Emprego
{
    public interface ICurriculumVitaeService : ICadastroMasterService<CurriculumVitae, int, CVExperiencia, CVExperienciaKey>
    {
        Task<CurriculumVitae> GetMyCV();
    }
}