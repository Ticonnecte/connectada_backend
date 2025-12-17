using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Entities.Model.Noticias;
using CidConnectada.Services.Intf.Account;
using CidConnectada.Services.Intf.AWS;
using CidConnectada.Services.Intf.Messaging;
using CidConnectada.Services.Intf.Noticias;
using CidConnectada.Webapi.Code.Extensions;
using CidConnectada.Webapi.Models.Noticias;
using CidConnectada.Website.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Zenite.Pi.Context;
using Zenite.Pi.Entities.Enums;
using Zenite.Pi.Entities.Model.Search;
using Zenite.Pi.Exceptions;
using Zenite.Pi.IoC;
using Zenite.Pi.Util.Control;
using Zenite.Pi.Util.Parallel;
using Zenite.Pi.Web.Models.Cadastro;
using Zenite.Pi.Web.Models.Pesquisa;
using Zenite.Pi.Web.WebApi;
using Zenite.Wa;
using Zenite.Wa.Models.Zapi.Message;

namespace CidConnectada.Webapi.Controllers.Noticias
{
    [ClaimsAuthorize]
    [RoutePrefix("api/Noticia")]
    public class NoticiaController : BaseWebApiController<Noticia, NoticiaDto,
        INoticiaService, string, int, string>
    {
        public NoticiaController(
            INoticiaService cadService,
            AutoMapper.IMapper mapper,
            Func<ContextRequest<int, string>> contextFactory,
            IZApiService zApiService,
            IAWSS3Service awss3Service

        )
            : base(cadService, mapper, contextFactory)
        {
            ZApiService = zApiService;
            AWSS3Service = awss3Service;
            GeneroEntidade = GenreEnum.Female;
            Title = "Notificação";
        }

        #region Services

        private readonly IAWSS3Service AWSS3Service;
        private readonly IZApiService ZApiService;


        #endregion

        private string BaseUrl => ((Usuario)Context.User)?.Prefeitura?.S3BaseUrl;
        #region Custom

        [HttpPost]
        [Route("EnviarNoticiaWpp")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public async Task<IHttpActionResult> EnviarNoticiaWpp(EnviarNoticiaWppDto model)
        {
            Noticia entity = await cadService.ObterAsync(model.key, new string[1]
            {
                "EnvioNoticiaSet.Usuario.AspNetUsers"
            });

            short ParallelismCoefficient = 4; //(Int16)Context.CacheRequest["Tenant:ParallelismCoefficient"];
            log.Info("Iniciando envio de mensagens...");

            if (!await ZApiService.ConnectedAsync())
            {
                throw new PiInfraException("Não há celular registrado na plataforma. Favor realizar leitura do QR-Code.");
            }
            
            if (ApplicationContext.AppSettings["Environment"] == nameof(ApiEnvironmentEnum.Development))
                return Ok("App em desenvolvimento. Nenhuma notícia enviada");

            if (entity.EnvioNoticiaSet.Any())
            {
                HttpContext httpCxt = HttpContext.Current;
                //string message = model.Text.Replace("~{Assinante.Nome}", SubscriberNameDefault);

                string apiBaseUrl = ApplicationContext.AppSettings["ApiBaseUrl"];
                await entity.EnvioNoticiaSet.Where(envioNoticia => !envioNoticia.DhEnvio.HasValue)
                    .ParallelForEachAsync(ParallelismCoefficient <= 0 ? 4 : ParallelismCoefficient, async envioNoticia =>
                    {
                        if (HttpContext.Current == null)
                        {
                            HttpContext.Current = httpCxt;
                        }

                        string phone = WhatsAppUtil.GetPhoneCleanUp(envioNoticia.Usuario.AspNetUsers.PhoneNumber);
                        string message = string.IsNullOrEmpty(model.mensagem) ? "" : $"{model.mensagem}\n";
                        message = message + $"{entity.Lead}" + $"\nLink: {apiBaseUrl}api/Noticia/Redirect?id={entity.Key}";

                        // Z-Api...
                        ZApiSendTextDto zApi = new ZApiSendTextDto
                        {
                            phone = phone,
                            message = message
                        };

                        bool exists = true;
                        //bool exists = !model.CheckExists;
                        //if (model.CheckExists)
                        //{
                        //    exists = await ZapiService.PhoneExistsAsync(phone);
                        //}

                        if (exists)
                        {
                            log.Info(String.Format("Enviando mensagem para o cliente '{0}'...", envioNoticia.Usuario.UserName));
                            ZApiMsgResultDto response = await ZApiService.SendMessageAsync(zApi);
                            //await cadService.SendedMessageRegister(envioNoticia, response.zaapId, response.messageId);
                        }
                    });

                return Ok($"Noticia enviada para {entity.EnvioNoticiaSet.Count(en => en.DhEnvio.HasValue)} conta(s) do Whatsapp.");
            }

            return Ok("Nenhuma noticia enviada.");
        }

        [HttpGet]
        [Route("Redirect")]
        public IHttpActionResult RedirectNoticia(string id)
        {
            //var userAgent = Request.Headers.UserAgent.ToString().ToLower();

            string appLink = $"{ApplicationContext.AppSettings["AppBaseUrl"]}/noticia/{id}";
            string fallbackUrl = ApplicationContext.AppSettings["AppPlayStoreLink"];

            // if (userAgent.Contains("iphone") || userAgent.Contains("ipad") || userAgent.Contains("mac os"))
            // {
            //     fallbackUrl = ApplicationContext.AppSettings["AppAppleStoreLink"];
            // }
            // else if (userAgent.Contains("android"))
            // {
            //     fallbackUrl = ApplicationContext.AppSettings["AppPlayStoreLink"];
            // }

            string htmlContent = $@"
            <!DOCTYPE html>
            <html lang='pt-BR'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Redirecionando...</title>
                <script>
                    var appLink = '{appLink}';
                    var fallbackUrl = '{fallbackUrl}';

                    // Tenta abrir o aplicativo
                    window.location.href = appLink;

                    // Se o app não abrir, após 2 segundos, redireciona para a Play Store
                    setTimeout(function () {{
                        window.location.href = fallbackUrl;
                    }}, 2000);
                </script>
            </head>
            <body>
                <p>Se o redirecionamento não funcionar, <a href='{appLink}'>clique aqui</a>.</p>
            </body>
            </html>";

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(htmlContent, Encoding.UTF8, "text/html")
            };

            return ResponseMessage(response);
        }

        #endregion

        #region CRUD

        [HttpPost]
        [Route("Post")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public override async Task<IHttpActionResult> Post(NoticiaDto model)
        {
            //if(await NoticiaCategoriaService.ExistsAsync(c => 
            //       model.categorias.Select(x => x.key).Contains(c.Key) && c.IndInfoSaibaMais) && model.categorias.Count > 1)
            //    return BadRequest("Só é possivel se adicionar mais de uma categoria, se nenhuma delas for 'Informativa'" /*Tem que arranjar um nome melhor pra isso*/);
            
            return await base.Post(model);
        }

        [HttpGet]
        [Route("GetOne")]
        [ResponseType(typeof(NoticiaDto))]
        public override async Task<IHttpActionResult> GetOne(string id)
        {
            return await base.GetOne(id);
        }

        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(IList<NoticiaDto>))]
        public override async Task<IHttpActionResult> GetAll()
        {
            return await base.GetAll();
        }

        [HttpPut]
        [Route("Put")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public override async Task<IHttpActionResult> Put(NoticiaDto model)
        {
            return await base.Put(model);
        }

        [HttpDelete]
        [Route("Delete")]
        [ClaimsAuthorize(Roles = "SA,ADMIN,FUNCIONARIO")]
        public override async Task<IHttpActionResult> Delete(string id)
        {
            Noticia entity = await cadService.ObterAsync(id);
            IList<S3Upload> s3DelList = entity.NoticiaImagesSet.Select(ii => new S3Upload()
            {
                Key = ii.ImgUrl.Substring(BaseUrl.Length),
                Remove = true
            }).ToList();
            Delegate deleteS3 = new Func<Noticia, Task>(async (ent) => await AWSS3Service.DeleteAsync(ent, s3DelList));
            await cadService.DeleteAsync(entity, deleteS3);

            return Ok();
        }

        [HttpGet]
        [Route("GetPage")]
        [ResponseType(typeof(SearchResultDto<NoticiaViewDto>))]
        public override async Task<IHttpActionResult> GetPage([FromUri] SearchOptions options)
        {
            return Ok(await base.GetPageGeneric<NoticiaViewDto>(options));
        }

        [HttpGet]
        [Route("GetFiltered")]
        [ResponseType(typeof(SearchResultDto<NoticiaViewDto>))]
        public override async Task<IHttpActionResult> GetFiltered([FromUri] ContainsFilter filter)
        {
            return Ok(await base.GetFilteredGeneric<NoticiaViewDto>(filter));
        }

        #endregion

        #region Custom
        
        protected async override Task IncluirAsync(Noticia entity)
        {
            IList<NoticiaImages> detail = entity.GetNewImages<NoticiaImages>(BaseUrl, HtmlHelper.ExtractImgSrcAttribute(entity.Conteudo));
            IList<S3Upload> s3Uploads = detail.Select(img => new S3Upload()
            {
                Key = img.ImgUrl.Substring(BaseUrl.Length),
                Base64 = img.Base64,
                Remove = false
            }).ToList();
            Delegate upload = new Func<object, Task>(async (ent) => await AWSS3Service.UploadS3Images((Noticia)ent, s3Uploads));
            entity.NoticiaImagesSet = new HashSet<NoticiaImages>(detail);
            await cadService.IncluirAsync(entity, upload);
        }

        protected async override Task AlterarAsync(Noticia entity)
        {
            IList<NoticiaImages> detail = entity.GetNewImages<NoticiaImages>(BaseUrl, HtmlHelper.ExtractImgSrcAttribute(entity.Conteudo));
            IList<S3Upload> s3Uploads = detail.Select(img => new S3Upload()
            {
                Key = img.ImgUrl.Substring(BaseUrl.Length),
                Base64 = img.Base64,
                Remove = false
            }).ToList();
            foreach (NoticiaImages oldImage in entity.NoticiaImagesSet)
            {
                if (entity.Conteudo.Contains(oldImage.Key.HashId.ToString()))
                {
                    NoticiaImages newImage = new NoticiaImages(oldImage.HashId, oldImage.ParentId);
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
            Delegate upload = new Func<object, Task>(async (ent) => await AWSS3Service.UploadS3Images((Noticia)ent, s3Uploads));
            await cadService.AlterarAsync(entity, new HashSet<NoticiaImages>(detail), upload);
        }
    }
}
#endregion
