using Amazon;
using Amazon.S3;
using CidConnectada.Entities.Model.AWS;
using CidConnectada.Entities.Model.Comercios;
using CidConnectada.Entities.Model.Infos;
using CidConnectada.Entities.Model.Organograma;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenite.Pi.Services;

namespace CidConnectada.Services.Intf.AWS
{
    public interface IAWSS3Service : IService
    {
        RegionEndpoint GetPrincipalRegionEndpoint();

        RegionEndpoint GetRegionEndpoint(string region);

        string GetPrincipalAccessKeyId();

        string GetPrincipalSecretAccessKey();
        AmazonS3Client GetS3Client(RegionEndpoint region = null, string accessKeyId = null, string secreteAccessKey = null, int connectionLimit = 50, int? bufferSize = null);

        Task<bool> CreateIamAsync(AmazonS3Client s3Client, Prefeitura prefeitura);

        Task UploadAsync<TEntity>(TEntity entity)
            where TEntity : S3FileGeneric;

        Task UploadS3Images<TEntity>(TEntity entity, IList<S3Upload> s3Uploads)
            where TEntity : S3FileGeneric;

        Task<bool> UploadAsync(string s3Key, string base64, string bucketName = null, string systemName = null, int connectionLimit = 50, int? bufferSize = null);

        Task<bool> UploadAsync(AmazonS3Client s3Client, string s3Key, string base64, string bucketName = null);
        Task<string> GetBase64Async(string s3Key, int? bufferSize = null);

        Task DeleteAsync<TEntity>(TEntity entity)
            where TEntity : S3FileGeneric;

        Task DeleteAsync<TEntity>(TEntity entity, IList<S3Upload> s3DelList)
                    where TEntity : S3FileGeneric;
        
        Task<bool> DeleteAsync(string s3Key, int connectionLimit = 50, int? bufferSize = null, string bucketName = null, string systemName = null);

        IList<KeyValuePair<string, string>> GetRegions();
    }
}
