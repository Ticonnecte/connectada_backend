using CidConnectada.Entities.Model.Organograma;
using CidConnectada.Services.Intf.Noticias;
using CidConnectada.Services.Intf.Organograma;
using CidConnectada.Webapi.Models.Banners;
using CidConnectada.Webapi.Models.Organograma;
using CidConnectada.Website.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Zenite.Pi.Context;
using Zenite.Pi.Entities.Enums;
using Zenite.Pi.Entities.Model.Search;
using Zenite.Pi.Web.Models.Cadastro;
using Zenite.Pi.Web.Models.Pesquisa;
using Zenite.Pi.Web.WebApi;

namespace CidConnectada.Webapi.Controllers.Organograma
{
    [RoutePrefix("api/Secretaria")]
    [ClaimsAuthorize]
    public class SecretariaController : MasterDetailWebApiController<Secretaria, SecretariaDto, ISecretariaService, string, SecretariaMenu, SecretariaMenuDto, SecretariaMenuKey,
        int, string>
    {
        public SecretariaController(ISecretariaService cadService, AutoMapper.IMapper mapper, Func<ContextRequest<int, string>> contextFactory)
            : base(cadService, mapper, contextFactory)
        {
            GeneroEntidade = GenreEnum.Female;
            Title = "Secretaria";
        }

        #region CRUD

        [HttpPost]
        [Route("Post")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        [ResponseType(typeof(SecretariaDto))]
        public override async Task<IHttpActionResult> Post(SecretariaDto model)
        {
            return await base.Post(model);
        }

        [HttpGet]
        [Route("GetOne")]
        [ResponseType(typeof(SecretariaDto))]
        public override async Task<IHttpActionResult> GetOne(string id)
        {
            return await base.GetOne(id);
        }

        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IList<SecretariaDto>))]
        public override async Task<IHttpActionResult> GetAll()
        {
            return await base.GetAll();
        }

        [HttpPut]
        [Route("Put")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        [ResponseType(typeof(SecretariaDto))]
        public override async Task<IHttpActionResult> Put(SecretariaDto model)
        {
            return await base.Put(model);
        }

        [HttpDelete]
        [Route("Delete")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public override async Task<IHttpActionResult> Delete(string id)
        {
            return await base.Delete(id);
        }

        [HttpGet]
        [Route("GetFiltered")]
        [ResponseType(typeof(SearchResultDto<SecretariaDto>))]
        public override async Task<IHttpActionResult> GetFiltered([FromUri] ContainsFilter filter)
        {
            return await base.GetFiltered(filter);
        }

        #endregion

        #region Custom

        [HttpGet]
        [Route("GetHome")]
        [ResponseType(typeof(IList<SecretariaDto>))]
        public async Task<IHttpActionResult> GetHome(int? qtde = null)
        {
            IList<Secretaria> secretarias = await cadService.GetHome(qtde);
            return Ok(AMapper.Map<IList<SecretariaDto>>(secretarias, opt => opt.Items["Caller"] = $"{nameof(SecretariaController)}.{nameof(GetHome)}"));
        }
        
        [HttpGet]
        [Route("GetActive")]
        [ResponseType(typeof(IList<SecretariaDto>))]
        public async Task<IHttpActionResult> GetActive()
        {
            IList<Secretaria> secretarias = await cadService.GetActive();
            return Ok(AMapper.Map<IList<SecretariaDto>>(secretarias));
        }

        [HttpGet]
        [Route("GetRotasInternas")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        [ResponseType(typeof(IList<RotaInternaDto>))]
        public async Task<IHttpActionResult> GetRotasInternas()
        {
            return Ok(AMapper.Map<IList<RotaInternaDto>>(await cadService.GetRotasInternasAsync()));
        }

        [HttpPut]
        [Route("AlterarOrdemHome")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        [ResponseType(typeof(IList<OrdemHomeDto<string>>))]
        public async Task<IHttpActionResult> AlterarOrdem(IList<OrdemHomeDto<string>> modelList)
        {
            await cadService.AlterarOrdemHome(modelList);
            IList<Secretaria> secretarias = await cadService.GetAllAsync();
            return Ok(AMapper.Map<IList<OrdemHomeDto<string>>>(secretarias));
        }

        [HttpGet]
        [Route("GetSecretariaList")]
        [ResponseType(typeof(IList<piLookupModel<string>>))]
        public async Task<IHttpActionResult> GetSecretariaList()
        {
            return Ok((await cadService.GetAllAsync()).Select(s => new piLookupModel<string> { value = s.Key, text = s.Nome, group = "Secretarias" }).ToList());
        }

        #endregion
    }
}