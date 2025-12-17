using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CidConnectada.Dao.Comercios;
using CidConnectada.Entities.Model.Comercios;
using CidConnectada.Services.Intf.Comercios;
using CidConnectada.Webapi.Models.Organograma;
using Zenite.Pi.Context;
using Zenite.Pi.Exceptions;
using Zenite.Pi.Services.Impl;

namespace CidConnectada.Services.Impl.Comercios
{
    public class TipoComercioService : CadastroMasterBaseService<TipoComercio, TipoComercioDao, int, CategoriaTipoComercio, int, CategoriaTipoComercioDao, int, string>, ITipoComercioService
    {

        public TipoComercioService(
            TipoComercioDao dao,
            Func<ContextRequest<int, string>> contextFactory, 
            CategoriaTipoComercioDao servDaoDetail1
        ) 
          : base(dao, contextFactory, servDaoDetail1  )
        {
        }
        #region Services e Dao


        #endregion


        #region CRUD
        public override string GetNomeEntidade(int indexDetail = 0)
        {
      
            return "Tipo Comércio";
        }

        public override object GetValorCampoDescritivoPadrao(TipoComercio entity)
        {
            return entity.Nome;
        }

        protected override Expression<Func<TipoComercio, bool>> GetUnicidadeFilter(TipoComercio entity)
        {
            return e => e.Nome == entity.Nome
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

        protected override string[] DeleteIncludes => new string[1] { "ComercioSet" };

        public override async Task<bool> CanDeleteAsync(TipoComercio entity)
        {
            bool hasComercio = entity.ComercioSet.Any();
            if (hasComercio)
            {
                throw new PiBusinessException("Operação abortada. Há Comércio(s) associado(s) a esse Tipo.");
            }
            else
            {
                return await base.CanDeleteAsync(entity);
            }
        }

        public async Task<IList<CategoriaTipoComercio>> GetCategoriasByTipoAsync(int key)
        {
            return await cadDaoDetail1.Where(e => e.TipoComercio.Key == key).ToListAsync();
        }

        public async Task<CategoriaTipoComercio> GetCategoriaAsync(int key)
        {
            return await cadDaoDetail1.FindByKeyAsync(key);
        }

        public CategoriaTipoComercio GetCategoria(int key)
        {
            return cadDaoDetail1.FindByKey(key);
        }
        
        public async Task<IList<TipoComercio>> GetHome(int? qtde = null)
        {
            IQueryable<TipoComercio> query = cadDao.Where(s => s.IsActive)
                .OrderByDescending(s => s.OrdemHome.HasValue)
                .ThenBy(s => s.OrdemHome)
                .ThenBy(s => s.Nome);

            if (qtde > 0)
                query = query.Take(qtde.Value);

            await query.LoadAsync();
            return await query.ToListAsync();
        }
        
        public async Task AlterarOrdemHome(IList<OrdemHomeDto<int>> ordemList)
        {
            IList<TipoComercio> entityList = await cadDao.AllAsync();

            IList<OrdemHomeDto<int>> ordens = ordemList.Where(o => entityList.All(e => e.Key != o.key)).ToList();

            if (ordens.Any())
            {
                string message = "Não foram encontrados registros no nosso sistema para os seguintes parâmetros: ";

                foreach (var ordem in ordens)
                    message += $@"[key: {ordem.key}] ";
                throw new PiBusinessException(message);
            }

            foreach (var entity in entityList)
            {
                var ordemDto = ordemList.FirstOrDefault(s => s.id == entity.Key);
                
                if (ordemDto == null)
                {
                    ordemDto = new OrdemHomeDto<int>
                    {
                        key = entity.Key,
                        nome = entity.Nome,
                    };
                    ordemList.Add(ordemDto);
                }
                else
                {
                    ordemDto.nome = entity.Nome;
                }
            }
            
            byte? index = ordemList.Max(s => s.ordemHome) ?? 0;
            foreach (var ordemDto in ordemList.Where(s => !s.ordemHome.HasValue).OrderBy(s => s.nome))
            {
                ordemDto.ordemHome = ++index;
            }

            await cadDao.AlterarOrdemHome(ordemList);
        }

        #endregion
    }
}
