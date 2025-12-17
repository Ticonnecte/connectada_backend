using Amazon;
using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;
using Amazon.S3;
using Amazon.S3.Internal;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using CidConnectada.Dao.Infos;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Infos;
using CidConnectada.Entities.Model.Organograma;
using CidConnectada.Services.Impl.Infos;
using CidConnectada.Services.Intf.AWS;
using CidConnectada.Services.Intf.Infos;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Zenite.Pi.Context;
using Zenite.Pi.Exceptions;
using Zenite.Pi.IoC;
using Zenite.Pi.Services;
using Zenite.Pi.Util.File;

namespace CidConnectada.Services.Impl.AWS
{
    public class AWSS3Service : IAWSS3Service
    {
        //private ContextRequestMultiTenancy<Int64, string, Int64> Context => (ContextRequestMultiTenancy<Int64, string, Int64>)ApplicationContext.ResolveContext<Request<Int64, string>>();
        protected readonly ILog log = LogManager.GetLogger(typeof(WindsorConfiguration));

        private readonly Func<ContextRequest<int, string>> _contextFactory;
        protected ContextRequest<int, string> Context => _contextFactory();
        public AWSS3Service(
            Func<ContextRequest<int, string>> contextFactory
        )
        {
            _contextFactory = contextFactory;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType is IService)
            {
                return ApplicationContext.Resolve(serviceType);
            }
            throw new PiInfraException(String.Format("'{0}' não implementa IService", serviceType.GetType().FullName));
        }

        #region Prefeitura
        protected AmazonS3Client S3Client { get; private set; }
        protected string PrincipalAccessKeyId => ApplicationContext.AppSettings["Amazon:S3:PrincipalAccessKeyId"];
        protected string PrincipalSecretAccessKey => ApplicationContext.AppSettings["Amazon:S3:PrincipalSecretAccessKey"];
        private string Dominio => ((Usuario)Context.User)?.Prefeitura?.Dominio;
        private string BucketName => ((Usuario)Context.User)?.Prefeitura?.BucketName;
        private string SystemName => ((Usuario)Context.User)?.Prefeitura?.S3Region;
        private string AccessKeyId => ((Usuario)Context.User)?.Prefeitura?.S3AccessKeyId;
        private string SecretAccessKey => ((Usuario) Context.User)?.Prefeitura?.S3AccessKeySecret;

        #endregion

        public RegionEndpoint GetPrincipalRegionEndpoint()
        {
            return GetRegionEndpoint(ApplicationContext.AppSettings["Amazon:S3:PrincipalRegion"]);
        }

        public RegionEndpoint GetRegionEndpoint(string region)
        {
            return RegionEndpoint.GetBySystemName(region);
        }

        public string GetPrincipalAccessKeyId()
        {
            return ApplicationContext.AppSettings["Amazon:S3:PrincipalAccessKeyId"];
        }

        public string GetPrincipalSecretAccessKey()
        {
            return ApplicationContext.AppSettings["Amazon:S3:PrincipalSecretAccessKey"];
        }

        public AmazonS3Client GetS3Client(RegionEndpoint region = null, string accessKeyId = null, string secreteAccessKey = null, int connectionLimit = 50, int? bufferSize = null)
        {
            return new AmazonS3Client(accessKeyId ?? AccessKeyId, secreteAccessKey ?? SecretAccessKey, new AmazonS3Config
            {
                ConnectionLimit = connectionLimit,
                BufferSize = bufferSize ?? Int32.Parse(ApplicationContext.AppSettings["Amazon:S3:BufferSize"]),
                RegionEndpoint = region ?? GetRegionEndpoint(SystemName)
            });
        }

        public async Task<bool> CreateIamAsync(AmazonS3Client s3Client, Prefeitura prefeitura)
        {
            try
            {
                var iamClient = new AmazonIdentityManagementServiceClient(
                    PrincipalAccessKeyId,
                    PrincipalSecretAccessKey,
                    GetPrincipalRegionEndpoint()
                );

                var request = new CreateAccessKeyRequest
                {
                    UserName = $"{Dominio}-{ApplicationContext.AppSettings["Amazon:S3:BucketSufixo"]}-user"
                };

                var response = await iamClient.CreateAccessKeyAsync(request);
                if (response.HttpStatusCode == HttpStatusCode.OK || response.HttpStatusCode == HttpStatusCode.Created)
                {
                    log.Info($"Prefeitura: ${prefeitura.Nome} | AccessKeyId: ${response.AccessKey.AccessKeyId} | SecretAccessKey: ${response.AccessKey.SecretAccessKey}");
                    prefeitura.S3AccessKeyId = response.AccessKey.AccessKeyId;
                    prefeitura.S3AccessKeySecret = response.AccessKey.SecretAccessKey;
                    return await CreateBucketAsync(s3Client, prefeitura, response.AccessKey.UserName);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exc)
            {
                Context.AddException(exc);
                throw;
            }
        }
        protected async Task<bool> CreateBucketAsync(AmazonS3Client s3Client, Prefeitura prefeitura, string IamUserName)
        {
            try
            {
                // Inicializa o cliente IAM
                var iamClient = new AmazonIdentityManagementServiceClient(prefeitura.S3AccessKeyId, prefeitura.S3AccessKeySecret, RegionEndpoint.GetBySystemName(prefeitura.S3Region));

                // Define o nome do novo usuário
                var request = new CreateUserRequest
                {
                    UserName = IamUserName
                };

                PutBucketRequest bucketRequest = new PutBucketRequest()
                {
                    BucketName = prefeitura.BucketName,
                    // S3Region US (us-east-1)
                    //BucketRegion = S3Region.FindValue(""),
                    BucketRegionName = prefeitura.S3Region,
                    CannedACL = S3CannedACL.PublicRead
                };
                PutBucketResponse response = await s3Client.PutBucketAsync(bucketRequest, Context.CancelToken);
                if (response.HttpStatusCode != HttpStatusCode.OK && response.HttpStatusCode != HttpStatusCode.Created)
                {
                    throw new PiInfraException($"Operação abortada. Erro na criação do S3 Bucket. Status: {response.HttpStatusCode}.");
                }
                return true;
            }
            catch (Exception exc)
            {
                Context.AddException(exc);
                return false;
            }
        }
        public IList<KeyValuePair<string, string>> GetRegions()
        {
            return RegionEndpoint.EnumerableAllRegions.Select(r => new KeyValuePair<string, string>(r.SystemName, r.DisplayName)).ToList();
        }
        public async Task<bool> DeleteAsync(AmazonS3Client S3Client, string s3Key, string bucketName = null)
        {
            DeleteObjectResponse response = await S3Client.DeleteObjectAsync(bucketName ?? BucketName, s3Key);
            return response.HttpStatusCode == HttpStatusCode.OK || response.HttpStatusCode == HttpStatusCode.NoContent;
        }
        public async Task<bool> DeleteAsync<TEntity>(AmazonS3Client S3Client, TEntity entity, Func<TEntity, bool, Task> excluirFunc)
            where TEntity : S3FileGeneric
        {
            string s3Key = entity.GetS3Key();
            bool result = true;
            try
            {
                DeleteObjectResponse response = await S3Client.DeleteObjectAsync(BucketName, s3Key);
                result = response.HttpStatusCode == HttpStatusCode.OK || response.HttpStatusCode == HttpStatusCode.NoContent;
                if (result)
                {
                    await excluirFunc(entity, true);
                }
                return result;
            }
            catch (Exception exc)
            {
                Context.AddException(exc);
                return false;
            }
        }

        //public async Task<bool> DeleteAsync(IList<string> s3Keys, int connectionLimit = 50, int? bufferSize = null)
        //{
        //    GetS3(connectionLimit, bufferSize, string.IsNullOrEmpty(SystemName) ? null : RegionEndpoint.GetBySystemName(SystemName));
        //    List<KeyVersion> objects = s3Keys.Select(a => new KeyVersion
        //    {
        //        Key = a
        //    }).ToList();
        //    DeleteObjectsRequest request = new DeleteObjectsRequest
        //    {
        //        BucketName = BucketName,
        //        Objects = objects
        //    };
        //    DeleteObjectsResponse response = await S3Client.DeleteObjectsAsync(request, Context.CancelToken);
        //    return response.HttpStatusCode == HttpStatusCode.OK || response.HttpStatusCode == HttpStatusCode.NoContent;
        //}

        public async Task UploadAsync<TEntity>(TEntity entity)
                where TEntity : S3FileGeneric
        {
            AmazonS3Client s3Client = GetS3Client();
            string s3Key = entity.GetS3Key();
            try
            {
                if (entity.CalculateHashCode() != 0 && !string.IsNullOrEmpty(entity._Base64))
                {
                    await UploadAsync(s3Client, s3Key, entity._Base64);
                }
                if (Context.CacheRequest.TryGetValue("OldExtension", out object oldExtension))
                {
                    await DeleteAsync(s3Client, entity.GetS3Key(oldExtension.ToString()));
                }
            }
            catch (Exception exc)
            {
                throw new PiInfraException(exc);
            }
        }

        public async Task UploadS3Images<TEntity>(TEntity entity, IList<S3Upload> s3Uploads)
            where TEntity : S3FileGeneric
        {
            try
            {
                await UploadAsync(entity);
                Random random = new Random();
                Parallel.ForEach(s3Uploads, async (s3Upload) =>
                {
                    Thread.Sleep(random.Next(180, 360));
                    if (s3Upload.Remove)
                    {
                        await DeleteAsync(s3Upload.Key);
                    }
                    else
                    {
                        await UploadAsync(s3Upload.Key, s3Upload.Base64);
                    }
                });
            }
            catch (Exception exc)
            {
                Context.AddException(exc);
            }
        }

        public async Task<bool> UploadAsync(string s3Key, string base64, string bucketName = null, string systemName = null, int connectionLimit = 50, int? bufferSize = null)
        {
            AmazonS3Client s3Client = GetS3Client();
            MemoryStream memoryStream = null;
            try
            {
                byte[] binary = null;
                if (!String.IsNullOrEmpty(base64))
                {
                    binary = Convert.FromBase64String(base64);
                    memoryStream = new MemoryStream(binary);
                    return await UploadAsync(s3Client, s3Key, memoryStream, bucketName);
                }
                else
                {
                    throw new PiInfraException("Conteúdo base64 não informado");
                }
            }
            catch (Exception exc)
            {
                throw new PiInfraException(exc);
            }
            finally
            {
                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                }
            }
        }
        public async Task<bool> UploadAsync(AmazonS3Client s3Client, string s3Key, string base64, string bucketName = null)
        {
            MemoryStream memoryStream = null;
            try
            {
                byte[] binary = null;
                if (!String.IsNullOrEmpty(base64))
                {
                    binary = Convert.FromBase64String(base64);
                    memoryStream = new MemoryStream(binary);
                    return await UploadAsync(s3Client, s3Key, memoryStream, bucketName);
                }
                else
                {
                    throw new PiInfraException("Conteúdo base64 não informado");
                }
            }
            catch (Exception exc)
            {
                throw new PiInfraException(exc);
            }
            finally
            {
                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                }
            }
        }

        public async Task<string> GetBase64Async(string s3Key, int? bufferSize = null)
        {
            TransferUtility transferUtility = new TransferUtility(AccessKeyId, SecretAccessKey, string.IsNullOrEmpty(SystemName) ? RegionEndpoint.USEast1 : RegionEndpoint.GetBySystemName(SystemName));
            Stream stream = await transferUtility.OpenStreamAsync(BucketName, s3Key);
            byte[] content = await FileUtil.GetContent(stream, bufferSize ?? Int32.Parse(ApplicationContext.AppSettings["Amazon:S3:BufferSize"]));
            return Convert.ToBase64String(content);
        }

        protected async Task EnsureBucketExistsAsync(string bucketName, TransferUtility transferUtility)
        {
            try
            {
                // Tenta localizar o bucket
                var location = await transferUtility.S3Client.GetBucketLocationAsync(bucketName);
            }
            catch (AmazonS3Exception ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound ||
                    ex.ErrorCode == "NoSuchBucket")
                {
                    // Se não existir, cria o bucket
                    await transferUtility.S3Client.EnsureBucketExistsAsync(bucketName);
                }
                else
                {
                    throw ex;
                }
            }
        }

        protected async Task<bool> UploadAsync(AmazonS3Client s3Client, string s3Key, MemoryStream memoryStream, string bucketName = null)
        {
            TransferUtility transferUtility = new TransferUtility(s3Client);
            if (string.IsNullOrEmpty(bucketName))
            {
                bucketName = BucketName;
            }
            try
            {
                await EnsureBucketExistsAsync(bucketName, transferUtility);
                await transferUtility.S3Client.UploadObjectFromStreamAsync(
                    bucketName,
                    s3Key,
                    memoryStream,
                    null,
                    Context.CancelToken
                );
                return true;
            }
            catch (Exception exc)
            {
                Context.AddException(new PiInfraException(exc));
                return false;
            }
            finally
            {
                if (transferUtility != null)
                {
                    transferUtility.Dispose();
                }
            }
        }


        public async Task<bool> DeleteAsync(string s3Key, int connectionLimit = 50, int? bufferSize = null, string bucketName = null, string systemName = null)
        {
            AmazonS3Client s3Client = GetS3Client(null, null, null, connectionLimit, bufferSize);
            try
            {
                return await DeleteAsync(s3Client, s3Key);
            }
            catch (Exception exc)
            {
                throw new PiInfraException(exc);
            }
        }

        public async Task DeleteAsync<TEntity>(TEntity entity)
            where TEntity : S3FileGeneric
        {
            AmazonS3Client s3Client = GetS3Client();
            string s3Key = entity.GetS3Key();
            try
            {
                if (!string.IsNullOrEmpty(s3Key))
                {
                    await DeleteAsync(s3Client, s3Key);
                }
            }
            catch (Exception exc)
            {
                throw new PiInfraException(exc);
            }
        }

        public async Task DeleteAsync<TEntity>(TEntity entity, IList<S3Upload> s3DelList)
            where TEntity : S3FileGeneric
        {
            await DeleteAsync(entity);
            AmazonS3Client s3Client = GetS3Client();
            Random random = new Random();
            Parallel.ForEach(s3DelList, async s3Del =>
            {
                Thread.Sleep(random.Next(180, 360));
                await DeleteAsync(s3Client, s3Del.Key);
            });
        }

    }
}
