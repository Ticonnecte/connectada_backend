using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Threading.Tasks;
using CidConnectada.Entities.Model.Dto.Location;
using CidConnectada.Entities.Model.Local;
using Zenite.Pi.Services;

namespace CidConnectada.Services.Intf.Local
{
    public interface IEnderecoService : ICadastroService<Endereco, long>
    {
        Task<Endereco> GetByCoordinatesAsync(DbGeography coordinates);
        Task<Endereco> GetByPlaceIdAsync(string placeId);
        Cidade GetCidade(int id);
        Cidade GetCidade(string nome, string estadoSigla);
        Task<IList<Bairro>> GetBairrosPorCidadeId(int id);
        Bairro GetBairro(int id);
        Bairro GetBairro(string nome, string cidadeNome);
        Task<Endereco> FindAddressByDetailsAsync(Endereco endereco);

        [TransactionRequired]
        Task<Endereco> GetAddressAsync(LocationDto model);
    }
}