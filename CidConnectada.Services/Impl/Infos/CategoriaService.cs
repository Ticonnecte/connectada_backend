using CidConnectada.Dao.Infos;
using CidConnectada.Entities.Model.Infos;
using CidConnectada.Services.Intf.Infos;
using Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Zenite.Pi.Context;
using Zenite.Pi.Services.Impl;

namespace CidConnectada.Services.Impl.Infos
{
    public class CategoriaService : CadastroBaseService<Categoria, CategoriaDao, int, int, string>, ICategoriaService
    {
        public CategoriaService(CategoriaDao _cadDao, Func<ContextRequest<int, string>> contextFactory) : base(_cadDao, contextFactory)
        {
        }

        public override string GetNomeEntidade(int indexDetail = 0)
        {
            return "Categoria";
        }

        public override object GetValorCampoDescritivoPadrao(Categoria entity)
        {
            return entity.Nome;
        }

        protected override Expression<Func<Categoria, bool>> GetUnicidadeFilter(Categoria entity)
        {
            return e => e.Nome == entity.Nome && e.Key != entity.Key;
        }

        protected async override Task<bool> CanDeleteDynamic(Categoria entity, bool isAsync = false)
        {
            bool result = true;
            if (entity.InfoSet.Any())
            {
                Context.AddExceptionMessage("Operação abordata.\nEsta categoria possui Informação(es) associada(s)");
                result = false;
            }
            return result;

        }

        #region Custom

        public async Task<IList<Categoria>> GetAtivasAsync()
        {
            return await cadDao.Where(c => c.Ativa, new string[1] { "InfoSet" }).ToListAsync();
        }

        protected override string[] DeleteIncludes => new string[1] { "InfoSet" };


        #endregion
    }
}
