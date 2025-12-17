using Amazon.S3;
using CidConnectada.Dao.Organograma;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Dto;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Entities.Model.Identity;
using CidConnectada.Entities.Model.Organograma;
using CidConnectada.Services.Intf.Account;
using CidConnectada.Services.Intf.AWS;
using CidConnectada.Services.Intf.Organograma;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Zenite.Pi.Context;
using Zenite.Pi.Entities.Model.Account;
using Zenite.Pi.Entities.Model.Common;
using Zenite.Pi.Exceptions;
using Zenite.Pi.Services.Impl;
using ApplicationContext = Zenite.Pi.IoC.ApplicationContext;

namespace CidConnectada.Services.Impl.Organograma
{
    public class PrefeituraService : CadastroBaseService<Prefeitura, PrefeituraDao, int, int, string>,
        IPrefeituraService
    {

        public PrefeituraService(PrefeituraDao cadDao, Func<ContextRequest<int, string>> contextFactory,
            IAWSS3Service aWSS3Service,
            IUsuarioService usuarioService
            )
            : base(cadDao, contextFactory)
        {
            AWSS3Service = aWSS3Service;
            UsuarioService = usuarioService;
        }



        #region Daos-Services
        private readonly IAWSS3Service AWSS3Service;
        private readonly IUsuarioService UsuarioService;


        #endregion

        #region CRUD

        public override string GetNomeEntidade(int indexDetail = 0)
        {
            return "Prefeitura";
        }

        public override object GetValorCampoDescritivoPadrao(Prefeitura entity)
        {
            return entity.Nome;
        }

        protected override Expression<Func<Prefeitura, bool>> GetUnicidadeFilter(Prefeitura entity)
        {
            return e => e.Nome == entity.Nome && e.Key != entity.Key;
        }

        #endregion

        #region Tenant

        public IList<PiLookup<int>> GetTenantList(string operationKey)
        {
            return cadDao.All().Select(e => new PiLookup<int> { Value = e.Key, Text = e.Nome, Group = "Prefeitura" })
                .ToList();
        }

        public Prefeitura ObterTenant(int key)
        {
            return cadDao.FindByKey(key);
        }

        public bool IsAuthorized<TUserOperationKey>(int key, IHieUser<TUserOperationKey> userEntity)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Custom

        public string GetAWSBaseUrl(Prefeitura entity)
        {
            return entity is null ? null : entity.S3BaseUrl;
        }

        public async Task<Prefeitura> IncluirPlusAsync(Prefeitura entity, Usuario user, ApplicationUser appUser, Delegate upload)
        {
            var imagens = new Dictionary<string, string>();

            //Criar IAM e Bucket para nova Prefeitura
            try
            {
                AmazonS3Client s3Client = AWSS3Service.GetS3Client(
                    AWSS3Service.GetPrincipalRegionEndpoint(),
                    AWSS3Service.GetPrincipalAccessKeyId(),
                    AWSS3Service.GetPrincipalSecretAccessKey()
                 );

                entity = await base.IncluirAsync(entity);
                Context.CacheRequest["TenantId"] = entity.Key;
                await CriarPastaTenantFront(entity, imagens);

                appUser.TenantKey = entity.Key;
                user.TenantKey = entity.Key;
                user.Prefeitura = entity;
                user.IndPrincipal = true;

                if (await AWSS3Service.CreateIamAsync(s3Client, entity))
                {
                    await UsuarioService.IncluirAdminAsync(user, appUser);
                }
                return entity;
            }
            catch (Exception)
            {
                string frontPath = ApplicationContext.AppSettings["FrontPath"];
                frontPath = $@"{frontPath}\tenants\{entity.Dominio}";
                if (Directory.Exists(frontPath))
                    Directory.Delete(frontPath, true);

                throw;
            }
        }

        // Vem pra cá após salvar no banco de dados, já com nova chave atribuída e sem erro(s) de banco, e antes do commit.
        // Ou seja, se gerar uma exceção as alterações do banco são desfeitas.
        public async Task UploadLogos(Prefeitura entity)
        {
            var imagens = new Dictionary<string, string>();

            try
            {
                AmazonS3Client s3Client = AWSS3Service.GetS3Client(
                    AWSS3Service.GetRegionEndpoint(entity.S3Region),
                    entity.S3AccessKeyId,
                    entity.S3AccessKeySecret
                 );

                string headerImgKey = entity.GetS3Key(LogoPrefeituraEnum.Header, null);
                if (!string.IsNullOrEmpty(entity.Base64LogoHeader))
                {
                    imagens.Add("logoHeader", entity.Base64LogoHeader);
                    await AWSS3Service.UploadAsync(s3Client, headerImgKey, entity.Base64LogoHeader, entity.BucketName);
                }

                string horizontalImgKey = entity.GetS3Key(LogoPrefeituraEnum.Horizontal, null);
                if (!string.IsNullOrEmpty(entity.LogoHorizontalUrl))
                {
                    imagens.Add("logoHorizontal", entity.Base64LogoHorizontal);
                    await AWSS3Service.UploadAsync(s3Client, horizontalImgKey, entity.Base64LogoHorizontal, entity.BucketName);
                }

                string verticalImgKey = entity.GetS3Key(LogoPrefeituraEnum.Vertical, null);
                if (!string.IsNullOrEmpty(entity.Base64LogoVertical))
                {
                    imagens.Add("logoVertical", entity.Base64LogoVertical);
                    await AWSS3Service.UploadAsync(s3Client, verticalImgKey, entity.Base64LogoVertical, entity.BucketName);
                }
            }
            catch (Exception)
            {
                //await AWSS3Service.DeleteAsync(headerImgKey, 50, null, entity.BucketName, entity.S3Region);
                //await AWSS3Service.DeleteAsync(horizontalImgKey, 50, null, entity.BucketName, entity.S3Region);
                //await AWSS3Service.DeleteAsync(verticalImgKey, 50, null, entity.BucketName, entity.S3Region);

                throw;
            }

        }


        private async Task CriarPastaTenantFront(Prefeitura entity, Dictionary<string, string> imagens)
        {
            FileStream imageStream = null;
            StreamContent imageContent = null;
            try
            {
                string frontPath = ApplicationContext.AppSettings["FrontPath"];
                if (!Directory.Exists(frontPath))
                    throw new PiBusinessException("Pasta do projeto de FrontEnd não encontrada");

                frontPath = $@"{frontPath}\tenants\{entity.Dominio}";
                if (!Directory.Exists(frontPath))
                    Directory.CreateDirectory(frontPath);

                ThemeDto theme = new ThemeDto(entity.PrimaryMainColor, entity.PrimaryDarkColor,
                    entity.PrimaryLightColor, entity.SecondaryMainColor,
                    entity.SecondaryDarkColor, entity.SecondaryLightColor);

                string themeContent = JsonConvert.SerializeObject(theme, Formatting.Indented);
                string themePath = $@"{frontPath}\theme.json";
                File.WriteAllText(themePath, themeContent);

                ConfigDto config = new ConfigDto(entity.Key);

                string configContent = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText($@"{frontPath}\config.json", configContent);

                if (imagens.Any())
                {
                    frontPath = $@"{frontPath}\images";

                    if (!Directory.Exists(frontPath))
                        Directory.CreateDirectory(frontPath);

                    byte[] bytes = null;
                    foreach (var item in imagens)
                    {
                        bytes = Convert.FromBase64String(item.Value);
                        imageContent = new StreamContent(new MemoryStream(bytes));

                        imageStream = File.Create($@"{frontPath}\{item.Key}.png");
                        await imageContent.CopyToAsync(imageStream);
                    }
                }
            }
            catch (FormatException)
            {
                throw new PiBusinessException(
                    "Operação Abortada: Não foi possível fazer a conversão do arquivo. Tente em instantes ou contate o suporte.");
            }
            catch (Exception e)
            {
                Context.AddExceptionMessage(e.Message);
                throw;
            }
            finally
            {
                imageStream?.Close();
                imageStream?.Dispose();
                imageContent?.Dispose();
            }
        }
        public  async Task UpdateRedesSociaisAsync(Prefeitura entity)
        {
          IsValid(entity);
        }

        protected override async Task AlterarDynamic(Prefeitura entity, bool async = false)
        {
            if (!string.IsNullOrEmpty(entity.Base64LogoHeader))
            {
                string headerImgKey = entity.GetS3Key(LogoPrefeituraEnum.Header, null);
                await AWSS3Service.DeleteAsync(headerImgKey, 50, null, entity.BucketName, entity.S3Region);
                await AWSS3Service.UploadAsync(headerImgKey, entity.Base64LogoHeader, entity.BucketName);
            }

            if (!string.IsNullOrEmpty(entity.LogoHorizontalUrl))
            {
                string horizontalImgKey = entity.GetS3Key(LogoPrefeituraEnum.Horizontal, null);
                await AWSS3Service.DeleteAsync(horizontalImgKey, 50, null, entity.BucketName, entity.S3Region);
                await AWSS3Service.UploadAsync(horizontalImgKey, entity.Base64LogoHorizontal, entity.BucketName);
            }

            if (!string.IsNullOrEmpty(entity.Base64LogoVertical))
            {
                string verticalImgKey = entity.GetS3Key(LogoPrefeituraEnum.Vertical, null);
                await AWSS3Service.DeleteAsync(verticalImgKey, 50, null, entity.BucketName, entity.S3Region);
                await AWSS3Service.UploadAsync(verticalImgKey, entity.Base64LogoVertical, entity.BucketName);
            }

            await base.AlterarDynamic(entity, async);
        }


        protected override async Task ExcluirDynamic(Prefeitura entity, bool async = false)
        {
            if (!string.IsNullOrEmpty(entity.Base64LogoHeader))
            {
                string headerImgKey = entity.GetS3Key(LogoPrefeituraEnum.Header, null);
                await AWSS3Service.DeleteAsync(headerImgKey, 50, null, entity.BucketName, entity.S3Region);
            }

            if (!string.IsNullOrEmpty(entity.LogoHorizontalUrl))
            {
                string horizontalImgKey = entity.GetS3Key(LogoPrefeituraEnum.Horizontal, null);
                await AWSS3Service.DeleteAsync(horizontalImgKey, 50, null, entity.BucketName, entity.S3Region);
            }

            if (!string.IsNullOrEmpty(entity.Base64LogoVertical))
            {
                string verticalImgKey = entity.GetS3Key(LogoPrefeituraEnum.Vertical, null);
                await AWSS3Service.DeleteAsync(verticalImgKey, 50, null, entity.BucketName, entity.S3Region);
            }

            await base.ExcluirDynamic(entity, async);
        }

        #endregion
    }
}
