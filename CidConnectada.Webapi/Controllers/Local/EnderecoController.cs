using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Dto.Location;
using CidConnectada.Entities.Model.Local;
using CidConnectada.Entities.Model.Organograma;
using CidConnectada.Services.Intf.Account;
using CidConnectada.Services.Intf.Infos;
using CidConnectada.Services.Intf.Local;
using CidConnectada.Services.Intf.Organograma;
using CidConnectada.Webapi.Models.Local;
using CidConnectada.Website.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Zenite.Pi.Context;
using Zenite.Pi.Entities.Enums;
using Zenite.Pi.Web.Models.Cadastro;
using Zenite.Pi.Web.Models.Pesquisa;
using Zenite.Pi.Web.WebApi;

namespace CidConnectada.Webapi.Controllers.Local
{
    [ClaimsAuthorize]
    [RoutePrefix("api/Endereco")]
    public class EnderecoController : BaseWebApiController<Endereco, EnderecoDto, IEnderecoService, long, int, string>
    {
        public EnderecoController(
            IEnderecoService cadService,
            AutoMapper.IMapper mapper,
            Func<ContextRequest<int, string>> contextFactory,
            IGAddressService gAddressService,
            IPrefeituraService prefeituraService
        )
            : base(cadService, mapper, contextFactory)
        {
            GAddressService = gAddressService;
            PrefeituraService = prefeituraService;
            GeneroEntidade = GenreEnum.Male;
            Title = "Endereço";
        }

        #region Services

        private readonly IGAddressService GAddressService;
        private readonly IPrefeituraService PrefeituraService;

        #endregion

        #region Custom

        [HttpGet]
        [Route("GetBairrosTenant")]
        [ResponseType(typeof(IList<piLookupModel<int>>))]
        public async Task<IHttpActionResult> GetBairrosTenant()
        {
            if (!Context.CacheRequest.TryGetValue("TenantId", out object tId) || !Int32.TryParse(tId.ToString(), out int tenantId))
                return BadRequest("O Header TenantId não foi informado ou é inválido.");

            Prefeitura tenant = PrefeituraService.Obter(tenantId, new string[1] { "Endereco.Cidade" });
            IList<Bairro> bairroList = await cadService.GetBairrosPorCidadeId(tenant.Endereco.Cidade.Key);
            return Ok(bairroList.Select(b => new piLookupModel<int> { value = b.Key, text = b.Nome, group = tenant.Endereco.Cidade.Nome }));
        }

        [HttpPost]
        [Route("PlaceAutoComplete")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO,CIDADAO")]
        [ResponseType(typeof(PlaceAutoCompleteResponseDto))]
        public async Task<IHttpActionResult> PlaceAutoComplete(PlaceAutoCompleteRequestDto model)
        {
            IsValid();
            if (model is null)
                return BadRequest("Operation Aborted: Dto Format is Invalid.");

            if (model.locationBias.center.lat == 0 && model.locationBias.center.lng == 0)
            {
                Prefeitura prefeitura = await PrefeituraService.ObterAsync(((Usuario)Context.User).Prefeitura.Key);
                model.locationBias.center = LocationDto.FromDbGeo(prefeitura.Endereco.Coordenadas);
                model.locationBias.radius = 10000;
                model.input += $", {prefeitura.Endereco.Cidade.Estado.Sigla}";
            }
            model.input += " - Brasil";
            var result = new PlaceAutoCompleteResponseDto
            {
                predictions = await GAddressService.PlaceAutoComplete(model)
            };

            return Ok(result);
        }

        [HttpPost]
        [Route("SelectPlaceByGPlaceId")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO,CIDADAO")]
        [ResponseType(typeof(EnderecoDto))]
        public async Task<IHttpActionResult> SelectPlace(SelectPlaceDto model)
        {
            IsValid();
            if (model is null)
                return BadRequest("Operation Aborted: Dto Format is Invalid.");

            Endereco endereco = await cadService.GetByPlaceIdAsync(model.placeId);
            if (endereco is null)
            {
                Endereco googleEndereco = AMapper.Map<Endereco>(await GAddressService.PlaceDetails(model.placeId, model.sessionToken));

                endereco = await cadService.FindAddressByDetailsAsync(googleEndereco) ?? await cadService.IncluirAsync(googleEndereco);
            }

            return Ok(AMapper.Map<EnderecoDto>(endereco));
        }

        [HttpPost]
        [Route("SelectPlaceByCoordinates")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO,CIDADAO")]
        [ResponseType(typeof(EnderecoDto))]
        public async Task<IHttpActionResult> SelectPlace(LocationDto model)
        {
            IsValid();
            if (model is null)
                return BadRequest("Operation Aborted: Dto Format is Invalid.");

            return Ok(AMapper.Map<EnderecoDto>(await cadService.GetAddressAsync(model)));
        }

        #endregion
    }
}