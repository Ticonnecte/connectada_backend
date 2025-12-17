using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Banners;
using CidConnectada.Entities.Model.Infos;
using CidConnectada.Services.Intf.AWS;
using CidConnectada.Services.Intf.Infos;
using CidConnectada.Webapi.Code.Extensions;
using CidConnectada.Webapi.Models.Infos;
using CidConnectada.Website.Filters;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Zenite.Pi.Context;
using Zenite.Pi.Entities.Enums;
using Zenite.Pi.Entities.Model.Search;
using Zenite.Pi.Util.Control;
using Zenite.Pi.Web.Models.Cadastro;
using Zenite.Pi.Web.Models.Pesquisa;
using Zenite.Pi.Web.WebApi;

namespace CidConnectada.Webapi.Controllers.Infos
{
    [ClaimsAuthorize]
    [RoutePrefix("api/Info")]
    public class InfoController : BaseWebApiController<Info, InfoDto,
        IInfoService, string, int, string>
    {
        public InfoController(
            IInfoService cadService,
            AutoMapper.IMapper mapper,
            Func<ContextRequest<int, string>> contextFactory,
            IAWSS3Service aWSS3Service
        )
            : base(cadService, mapper, contextFactory)
        {
            AWSS3Service = aWSS3Service;
            GeneroEntidade = GenreEnum.Female;
            Title = "Info";
        }

        private readonly IAWSS3Service AWSS3Service;

        private string BaseUrl => ((Usuario)Context.User)?.Prefeitura?.S3BaseUrl;

        #region CRUD

        [HttpPost]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO,CIDADAO")]
        [Route("Post")]
        public override async Task<IHttpActionResult> Post(InfoDto model)
        {
            return await base.Post(model);
        }

        [HttpGet]
        [Route("GetOne")]
        [ResponseType(typeof(InfoDto))]
        public override async Task<IHttpActionResult> GetOne(string id)
        {
            return await base.GetOne<InfoDto>(id);
        }

        [HttpGet]
        [Route("GetAll")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO,CIDADAO")]
        [ResponseType(typeof(IList<InfoDto>))]
        public override async Task<IHttpActionResult> GetAll()
        {
            return await base.GetAll();
        }

        [HttpPut]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO,CIDADAO")]
        [Route("Put")]
        public override async Task<IHttpActionResult> Put(InfoDto model)
        {
            return await base.Put(model);
        }

        [HttpDelete]
        [Route("Delete")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public override async Task<IHttpActionResult> Delete(string id)
        {
            Info entity = await cadService.ObterAsync(id);
            IList<S3Upload> s3DelList = entity.InfoImagesSet.Select(ii => new S3Upload()
            {
                Key = ii.ImgUrl.Substring(BaseUrl.Length),
                Remove = true
            }).ToList();
            Delegate deleteS3 = new Func<Info, Task>(async (ent) => await AWSS3Service.DeleteAsync(ent, s3DelList));
            await cadService.DeleteAsync(entity, deleteS3);

            return Ok();
        }
        [HttpGet]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO,CIDADAO")]
        [Route("GetPage")]
        [ResponseType(typeof(SearchResultDto<InfoDto>))]
        public override async Task<IHttpActionResult> GetPage([FromUri] SearchOptions options)
        {
            return Ok(await base.GetPageGeneric<InfoDto>(options));
        }

        [HttpGet]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO,CIDADAO")]
        [Route("GetFiltered")]
        [ResponseType(typeof(SearchResultDto<InfoDto>))]
        public override async Task<IHttpActionResult> GetFiltered([FromUri] ContainsFilter filter)
        {
            return Ok(await base.GetFilteredGeneric<InfoDto>(filter));
        }

        #endregion

        #region Custom

        protected async override Task IncluirAsync(Info entity)
        {
            IList<InfoImages> detail = entity.GetNewImages<InfoImages>(BaseUrl, HtmlHelper.ExtractImgSrcAttribute(entity.Conteudo));
            IList<S3Upload> s3Uploads = detail.Select(img => new S3Upload()
            {
                Key = img.ImgUrl.Substring(BaseUrl.Length),
                Base64 = img.Base64,
                Remove = false
            }).ToList();
            Delegate upload = new Func<object, Task>(async (ent) => await AWSS3Service.UploadS3Images((Info)ent, s3Uploads));
            entity.InfoImagesSet = new HashSet<InfoImages>(detail);
            await cadService.IncluirAsync(entity, upload);
        }

        protected async override Task AlterarAsync(Info entity)
        {
            IList<InfoImages> detail = entity.GetNewImages<InfoImages>(BaseUrl, HtmlHelper.ExtractImgSrcAttribute(entity.Conteudo));
            IList<S3Upload> s3Uploads = detail.Select(img => new S3Upload()
            {
                Key = img.ImgUrl.Substring(BaseUrl.Length),
                Base64 = img.Base64,
                Remove = false
            }).ToList();
            foreach (InfoImages oldImage in entity.InfoImagesSet)
            {
                if (entity.Conteudo.Contains(oldImage.Key.HashId.ToString()))
                {
                    InfoImages newImage = new InfoImages(oldImage.HashId, oldImage.ParentId);
                    oldImage.CopyProperties(newImage);
                    detail.Add(newImage);
                }
                else
                {
                    s3Uploads.Add(new S3Upload()
                    {
                        Key = oldImage.ImgUrl.Substring(BaseUrl.Length),
                        Remove = true
                    });
                }
            }
            Delegate upload = new Func<object, Task>(async (ent) => await AWSS3Service.UploadS3Images((Info)ent, s3Uploads));
            await cadService.AlterarAsync(entity, new HashSet<InfoImages>(detail), upload);
        }

        #endregion

    }
}
