using CidConnectada.Dao.Emprego;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Comercios;
using CidConnectada.Entities.Model.Emprego;
using CidConnectada.Services.Intf.Emprego;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Zenite.Pi.Context;
using Zenite.Pi.Services.Impl;

namespace CidConnectada.Services.Impl.Emprego
{
    public class CurriculumVitaeService : CadastroMasterBaseService<CurriculumVitae, CurriculumVitaeDao, int, CVExperiencia, CVExperienciaKey, CVExperienciaDao, int, string>, ICurriculumVitaeService
    {
        public CurriculumVitaeService(CurriculumVitaeDao dao, Func<ContextRequest<int, string>> contextFactory, CVExperienciaDao servDaoDetail1) : base(dao, contextFactory, servDaoDetail1)
        {
        }
        #region CRUD

        public override string GetNomeEntidade(int indexDetail = 0)
        {
            return "Currículo";
        }

        public override object GetValorCampoDescritivoPadrao(CurriculumVitae entity)
        {
            return $"Cidadao: {entity.Cidadao.NomeCompleto}";
        }

        protected override Expression<Func<CurriculumVitae, bool>> GetUnicidadeFilter(CurriculumVitae entity)
        {
            return e => e.Cidadao.Key == entity.Cidadao.Key
                && e.Key != entity.Key;
        }

        protected override bool MustCascade(int indexDetail)
        {
            return true;
        }

        protected override bool RecordIsRequired(int indexDetail)
        {
            return false;
        }

        #endregion

        #region Custom

        protected override Task<CurriculumVitae> IncluirDynamic(CurriculumVitae entity, bool isAsync)
        {
            return base.IncluirDynamic(entity, isAsync);
        }

        protected override Task AlterarDynamic(CurriculumVitae entity, bool async = false)
        {
            return base.AlterarDynamic(entity, async);
        }

        public async Task<CurriculumVitae> GetMyCV()
        {
            int userKey = ((Usuario)Context.User).Key;
            return await cadDao.FirstOrDefaultAsync(cv => cv.Cidadao.Key == userKey);
        }

        public override async Task<bool> CanDeleteAsync(CurriculumVitae entity)
        {
            return await base.CanDeleteAsync(entity) && CheckUser(entity);
        }


        public override async Task<bool> IsValidAsync(CurriculumVitae entity, bool validateAllProperties = true)
        {
            return await base.IsValidDynamic(entity, validateAllProperties) && CheckUser(entity); ;
        }

        protected bool CheckUser(CurriculumVitae entity)
        {
            bool result = true;
            if (((Usuario)Context.User).Key != entity.Cidadao.Key && !Context.IsAdmin)
            {
                Context.AddExceptionMessage("Operação Abortada: apenas Administradores podem alterar/deletar o CV de outro cidadão.");
                result = false;
            }
            return result;
        }


        #endregion
    }
}
