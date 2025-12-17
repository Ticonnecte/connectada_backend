using CidConnectada.Dao.Comunicacao;
using CidConnectada.Entities.Model.Banners;
using CidConnectada.Entities.Model.Comunicacao;
using CidConnectada.Services.Intf.AWS;
using CidConnectada.Services.Intf.Comunicacao;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Zenite.Pi.Context;
using Zenite.Pi.Services.Impl;

namespace CidConnectada.Services.Impl.Comunicacao
{
    public class AgendaCulturalService : CadastroBaseService<AgendaCultural, AgendaCulturalDao, string, int, string>, IAgendaCulturalService
    {
        public AgendaCulturalService(AgendaCulturalDao _cadDao, Func<ContextRequest<int, string>> contextFactory) : base(_cadDao, contextFactory)
        {
        }

        #region CRUD

        public override string GetNomeEntidade(int indexDetail = 0)
        {
            return "Agenda Cultural";
        }

        public override object GetValorCampoDescritivoPadrao(AgendaCultural entity)
        {
            return entity.Titulo;
        }

        protected override Expression<Func<AgendaCultural, bool>> GetUnicidadeFilter(AgendaCultural entity)
        {
            return e => e.Titulo == entity.Titulo && e.Key != entity.Key;
        }

        #endregion

        #region Custom

        public async Task<AgendaCultural> IncluirAsync(AgendaCultural entity, Delegate upload)
        {
            return await IncluirAsync(entity);
        }

        public async Task AlterarAsync(AgendaCultural entity, Delegate upload)
        {
            await AlterarAsync(entity);
        }

        public async Task DeleteAsync(AgendaCultural entity, Delegate deleteS3)
        {
            await ExcluirAsync(entity);
        }

        #endregion
    }
}