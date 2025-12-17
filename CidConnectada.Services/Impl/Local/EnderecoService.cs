using AutoMapper;
using CidConnectada.Dao.Local;
using CidConnectada.Entities.Model.Dto.Location;
using CidConnectada.Entities.Model.Local;
using CidConnectada.Services.Intf.Local;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Zenite.Pi.Context;
using Zenite.Pi.IoC;
using Zenite.Pi.Services.Impl;

namespace CidConnectada.Services.Impl.Local
{
    public class EnderecoService : CadastroBaseService<Endereco, EnderecoDao, long, int, string>, IEnderecoService
    {
        public EnderecoService(
            EnderecoDao _cadDao,
            Func<ContextRequest<int, string>> contextFactory,
            CidadeDao cidadeDao,
            BairroDao bairroDao,
            IGAddressService gAddressService
        )
          : base(_cadDao, contextFactory)
        {
            CidadeDao = cidadeDao;
            BairroDao = bairroDao;
            GAddressService = gAddressService;
        }
        #region Services and Daos

        protected readonly CidadeDao CidadeDao;
        protected readonly BairroDao BairroDao;
        protected readonly IGAddressService GAddressService;

        protected IMapper AMapper => ApplicationContext.Resolve<IMapper>();

        //    protected IGAddressService GAddressService => ApplicationContext.Resolve<IGAddressService>();

        #endregion

        #region CRUD

        public override string GetNomeEntidade(int indexDetail = 0)
        {
            return "Endereço";
        }

        public override object GetValorCampoDescritivoPadrao(Endereco entity)
        {
            return $"{entity.EnderecoCompleto}";
        }

        protected override Expression<Func<Endereco, bool>> GetUnicidadeFilter(Endereco entity)
        {
            return e => e.Cidade.Key == entity.Cidade.Key
                && e.Rua == entity.Rua
                && e.Numero == entity.Numero
                && e.Key != entity.Key;
        }

        #endregion

        #region Custom

        public async Task<Endereco> GetByCoordinatesAsync(DbGeography coordinates)
        {
            double radius = Double.Parse(ApplicationContext.AppSettings["Geography:Location:Radius"]);
            //return await cadDao.SingleOrDefaultAsync(a => a.Coordinates.Intersects(coordinates), null);
            return await cadDao.FirstOrDefaultAsync(a => a.Coordenadas.Distance(coordinates) <= radius);
        }

        public async Task<Endereco> GetByPlaceIdAsync(string placeId)
        {
            IList<Endereco> addressList = await cadDao.Where(a => a.GoogleMapsPlaceId == placeId).ToListAsync();
            return addressList.FirstOrDefault();
        }

        public Cidade GetCidade(int id)
        {
            return CidadeDao.FindByKey(id);
        }

        public Bairro GetBairro(int id)
        {
            return BairroDao.FindByKey(id);
        }

        public async Task<IList<Bairro>> GetBairrosPorCidadeId(int id)
        {
            return await BairroDao.Where(b => b.Cidade.Key == id).ToListAsync();
        }

        public Cidade GetCidade(string nome, string estadoSigla)
        {
            Cidade cidade = CidadeDao.SingleOrDefault(c =>
                c.Nome == nome.ToUpper() && c.Estado.Sigla == estadoSigla.ToUpper()
            );
            return cidade;
        }

        public Bairro GetBairro(string nome, string cidadeNome)
        {
            Bairro cidade = BairroDao.SingleOrDefault(b =>
                b.Nome == nome.ToUpper() && b.Cidade.Nome == cidadeNome.ToUpper()
            );
            return cidade;
        }

        public async Task<Endereco> FindAddressByDetailsAsync(Endereco endereco)
        {
            return await cadDao.SingleOrDefaultAsync(e => e.Cidade.Key == endereco.Cidade.Key
                && e.Rua == endereco.Rua
                && e.Numero == endereco.Numero);
        }

        public async Task<Endereco> GetAddressAsync(LocationDto location)
        {
            try
            {
                DbGeography coordenadas = AMapper.Map<DbGeography>(location);
                Endereco endereco = await GetByCoordinatesAsync(coordenadas);
                if (endereco is null)
                {
                    EnderecoDto result = await GAddressService.GeoDecode(coordenadas);
                    endereco = await IncluirAsync(AMapper.Map<Endereco>(result));
                }
                return endereco;
            }
            catch (Exception exc)
            {
                Context.AddException(exc);
                return null;
            }
        }

        public async Task<Endereco> GetAddressAsync(string placeId, string sessionToken = null)
        {
            try
            {
                Endereco endereco = await GetByPlaceIdAsync(placeId);

                if (endereco is null)
                {
                    EnderecoDto gEnderecoDto = await GAddressService.PlaceDetails(placeId, sessionToken);

                    Endereco gEndereco = AMapper.Map<Endereco>(gEnderecoDto);
                    endereco = await FindAddressByDetailsAsync(gEndereco);
                    if (endereco is null)
                    {
                        endereco = await IncluirAsync(gEndereco);
                    }
                }

                return endereco;
            }
            catch (Exception exc)
            {
                Context.AddException(exc);
                return null;
            }
        }

        #endregion
    }
}
