using CidConnectada.Entities.Model.AWS;
using System;
using System.Threading.Tasks;
using Zenite.Pi.Services;

namespace CidConnectada.Services.Intf.AWS
{
    public interface IS3FileServiceGeneric<TEntity> : ICadastroService<TEntity, string>
        where TEntity : S3FileGeneric
    {
        [TransactionRequired]
        Task<TEntity> IncluirAsync(TEntity entity, Delegate upload);

        [TransactionRequired]
        Task AlterarAsync(TEntity entity, Delegate upload);

        [TransactionRequired]
        Task DeleteAsync(TEntity entity, Delegate deleteS3);
    }
}
