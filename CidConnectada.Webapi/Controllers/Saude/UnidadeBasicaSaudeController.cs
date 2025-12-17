using CidConnectada.Entities.Model.Banners;
using CidConnectada.Entities.Model.Saude;
using CidConnectada.Services.Impl.AWS;
using CidConnectada.Services.Intf.AWS;
using CidConnectada.Services.Intf.Organograma;
using CidConnectada.Services.Intf.Saude;
using CidConnectada.Webapi.Models.Common;
using CidConnectada.Webapi.Models.Saude;
using CidConnectada.Website.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using Zenite.Pi.Context;
using Zenite.Pi.Entities.Enums;
using Zenite.Pi.Entities.Model.Search;
using Zenite.Pi.Web.Models.Cadastro;
using Zenite.Pi.Web.Models.Pesquisa;
using Zenite.Pi.Web.WebApi;

namespace CidConnectada.Webapi.Controllers.Saude
{
    [ClaimsAuthorize]
    [RoutePrefix("api/UnidadeBasicaSaude")]
    public class UnidadeBasicaSaudeController : BaseWebApiController<UnidadeBasicaSaude, UbsDto, IUnidadeBasicaSaudeService, string, int, string>
    {

        private readonly IAWSS3Service AWSS3Service;

        public UnidadeBasicaSaudeController(
            IUnidadeBasicaSaudeService cadService,
            AutoMapper.IMapper mapper, Func<ContextRequest<int, string>> contextFactory,
            IAWSS3Service aWSS3Service
        )
            : base(cadService, mapper, contextFactory)
        {
            AWSS3Service = aWSS3Service;
            GeneroEntidade = GenreEnum.Female;
            Title = "UnidadeBasicaSaude";
        }

        private UbsDto _model;

        #region Custom

        [HttpGet]
        [Route("GetAllEspecialidadeMedica")]
        [ResponseType(typeof(IList<DetailDto>))]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public async Task<IHttpActionResult> GetAllEspecialidadeMedica()
        {
            return Ok((await cadService.GetAllEspecialidadeMedica()).Select(em => AMapper.Map<DetailDto>(em)));
        }

        [HttpGet]
        [Route("GetAllServicoSaude")]
        [ResponseType(typeof(IList<DetailDto>))]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public async Task<IHttpActionResult> GetAllServicoSaude()
        {
            return Ok((await cadService.GetAllServicoSaude()).Select(ss => AMapper.Map<DetailDto>(ss)));
        }

        #endregion

        #region CRUD

        protected override object GetPutResult<TDto>(UnidadeBasicaSaude entity)
        {
            return AMapper.Map<UbsViewDto>(entity);
        }

        protected override object GetPostResult<TDto>(UnidadeBasicaSaude entity)
        {
            return AMapper.Map<UbsViewDto>(entity);
        }

        [HttpPost]
        [Route("Post")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public override async Task<IHttpActionResult> Post(UbsDto model)
        {
            _model = model;
            return await base.Post(model);
        }

        [HttpGet]
        [Route("GetOne")]
        [ResponseType(typeof(UbsDto))]
        public override async Task<IHttpActionResult> GetOne(string id)
        {
            return await base.GetOne(id);
        }

        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IList<UbsViewDto>))]
        public override async Task<IHttpActionResult> GetAll()
        {
            IList<UbsViewDto> result = new List<UbsViewDto>();
            IList<UnidadeBasicaSaude> ubsList = await cadService.GetAllAsync();
            foreach (UnidadeBasicaSaude ubs in ubsList)
            {
                result.Add(EntityToModel<UbsViewDto>(ubs, nameof(GetAll)));
            }
            return Ok(result);
        }

        [HttpPut]
        [Route("Put")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public override async Task<IHttpActionResult> Put(UbsDto model)
        {
            _model = model;
            return await base.Put(model);
        }

        [HttpGet]
        [Route("GetPage")]
        [ResponseType(typeof(SearchResultDto<UbsViewDto>))]
        public override async Task<IHttpActionResult> GetPage([FromUri] SearchOptions options)
        {
            return Ok(await base.GetPageGeneric<UbsViewDto>(options));
        }

        [HttpGet]
        [Route("GetFiltered")]
        [ResponseType(typeof(SearchResultDto<UbsViewDto>))]
        public override async Task<IHttpActionResult> GetFiltered([FromUri] ContainsFilter filter)
        {
            return Ok(await base.GetFilteredGeneric<UbsViewDto>(filter));
        }

        #endregion

        #region Custom
        protected async override Task IncluirAsync(UnidadeBasicaSaude entity)
        {
            try
            {
                entity.UbsEspecialidadeMedicaSet = AMapper.Map<ISet<UbsEspecialidadeMedica>>(_model.especialidadeMedicaList, opt => opt.Items.Add("Ubs", entity));
                entity.UbsServicoSaudeSet = AMapper.Map<ISet<UbsServicoSaude>>(_model.servicoSaudeList, opt => opt.Items.Add("Ubs", entity));
                Delegate upload = new Func<object, Task>(async (ent) => await AWSS3Service.UploadAsync((UnidadeBasicaSaude)ent));
                await cadService.IncluirAsync(entity, upload);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        protected async override Task AlterarAsync(UnidadeBasicaSaude entity)
        {
            ISet<UbsEspecialidadeMedica> detail1 = AMapper.Map<ISet<UbsEspecialidadeMedica>>(_model.especialidadeMedicaList, opt => opt.Items.Add("Ubs", entity));
            ISet<UbsServicoSaude> detail2 = AMapper.Map<ISet<UbsServicoSaude>>(_model.servicoSaudeList, opt => opt.Items.Add("Ubs", entity));
            Delegate upload = new Func<object, Task>(async (ent) => await AWSS3Service.UploadAsync((UnidadeBasicaSaude)ent));
            await cadService.AlterarAsync(entity, detail1, detail2, upload);
        }

        [HttpDelete]
        [Route("Delete")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public override async Task<IHttpActionResult> Delete(string id)
        {
            UnidadeBasicaSaude entity = await cadService.ObterAsync(id);
            Delegate deleteS3 = new Func<object, Task>(async (ent) => await AWSS3Service.DeleteAsync((UnidadeBasicaSaude)ent));
            await cadService.DeleteAsync(entity, deleteS3);

            return Ok();
        }

        #endregion
    }
}