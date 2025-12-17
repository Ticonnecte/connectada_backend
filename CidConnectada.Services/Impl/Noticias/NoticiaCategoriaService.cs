using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CidConnectada.Dao.Noticias;
using CidConnectada.Entities.Model.Noticias;
using CidConnectada.Services.Intf.Noticias;
using Zenite.Pi.Context;
using Zenite.Pi.Services.Impl;

namespace CidConnectada.Services.Impl.Noticias
{
    public class NoticiaCategoriaService :
        CadastroBaseService<NoticiaCategoria, NoticiaCategoriaDao, int, int, string>, INoticiaCategoriaService
    {
        public NoticiaCategoriaService(
            NoticiaCategoriaDao cadDao,
            Func<ContextRequest<int, string>> contextFactory,
            NoticiaCategoriaVincDao noticiaCategoriaVincDao
        )
            : base(cadDao, contextFactory)
        {
            NoticiaCategoriaVincDao = noticiaCategoriaVincDao;
        }

        #region Daos

        protected readonly NoticiaCategoriaVincDao NoticiaCategoriaVincDao;

        #endregion


        #region CRUD

        public override string GetNomeEntidade(int indexDetail = 0)
        {
            return "Categoria de Notícia";
        }

        public override object GetValorCampoDescritivoPadrao(NoticiaCategoria entity)
        {
            return $"Nome: {entity.Nome}";
        }

        protected override Expression<Func<NoticiaCategoria, bool>> GetUnicidadeFilter(NoticiaCategoria entity)
        {
            return e => e.Nome == entity.Nome && e.Key != entity.Key;
        }

        #endregion
        
        #region Custom

        public IList<NoticiaCategoria> GetByNoticiaId(string idNoticia)
        {
            IList<NoticiaCategoriaVinc> vinculos = NoticiaCategoriaVincDao.Where(v => v.NoticiaId == idNoticia).ToList();
            IList<NoticiaCategoria> categorias = vinculos.Select(v => v.NoticiaCategoria).ToList();
            return categorias;
        }

        #endregion
        
    }
}
