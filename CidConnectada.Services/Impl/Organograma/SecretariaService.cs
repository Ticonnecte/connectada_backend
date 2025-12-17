using CidConnectada.Dao.Banners;
using CidConnectada.Dao.Organograma;
using CidConnectada.Entities.Model.Banners;
using CidConnectada.Entities.Model.Organograma;
using CidConnectada.Services.Intf.Organograma;
using CidConnectada.Webapi.Models.Organograma;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Zenite.Pi.Context;
using Zenite.Pi.Exceptions;
using Zenite.Pi.Services.Impl;

namespace CidConnectada.Services.Impl.Organograma
{
    public class SecretariaService : CadastroMasterBaseService<Secretaria, SecretariaDao, string, SecretariaMenu, SecretariaMenuKey, SecretariaMenuDao, int, string>, ISecretariaService
    {

        public SecretariaService(SecretariaDao cadDao, Func<ContextRequest<int, string>> contextFactory, SecretariaMenuDao secretariaMenuDao,
            RotaInternaDao rotaInternaDao
            )
            : base(cadDao, contextFactory, secretariaMenuDao)
        {
            RotaInternaDao = rotaInternaDao;
        }

        #region Daos
        private readonly RotaInternaDao RotaInternaDao;
        #endregion

        #region CRUD

        public override string GetNomeEntidade(int indexDetail = 0)
        {
            return "Secretaria";
        }

        public override object GetValorCampoDescritivoPadrao(Secretaria entity)
        {
            return entity.Nome;
        }

        protected override Expression<Func<Secretaria, bool>> GetUnicidadeFilter(Secretaria entity)
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

        public async Task<IList<Secretaria>> GetHome(int? qtde = null)
        {
            IQueryable<Secretaria> query = cadDao.Where(s => s.IsActive)
                .OrderByDescending(s => s.OrdemHome.HasValue)
                .ThenBy(s => s.OrdemHome)
                .ThenBy(s => s.Nome);

            if (qtde > 0)
                query = query.Take(qtde.Value);

            return await query.ToListAsync();
        }

        public async Task<IList<Secretaria>> GetActive()
        {
            IList<Secretaria> result = await cadDao.Where(s => s.IsActive).ToListAsync();
            return result;
        }

        public async Task AlterarOrdemHome(IList<OrdemHomeDto<string>> ordemList)
        {
            IList<Secretaria> entityList = await cadDao.AllAsync();

            IList<OrdemHomeDto<string>> ordens = ordemList.Where(o => entityList.All(e => e.Key != o.key)).ToList();

            if (ordens.Any())
            {
                string message = "Não foram encontrados registros no nosso sistema para os seguintes pârametros: ";

                foreach (var ordem in ordens)
                    message += $@"[key: {ordem.key}] ";
                throw new PiBusinessException(message);
            }

            foreach (var entity in entityList)
            {
                var ordemDto = ordemList.FirstOrDefault(s => s.id == entity.Key);

                if (ordemDto == null)
                {
                    ordemDto = new OrdemHomeDto<string>
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

        public async Task<IList<RotaInterna>> GetRotasInternasAsync()
        {
            return await RotaInternaDao.Where(r => r.EhSecretaria).ToListAsync();
        }

        public override Task<bool> CanDeleteAsync(Secretaria entity)
        {
            IList<string> msgList = new List<string>();
            bool result = entity.SecretariaMenuSet.Any();
            if (result)
            {
                msgList.Add("Operação abortada. A Secretaria não pode ser deletada pois há Menu(s) associada(s) a ela.");
            }
            if (entity.DialogoPreConfigSet.Any())
            {
                result = true;
                msgList.Add("Operação abortada. A Secretaria não pode ser deletada pois há Diálogo(s) pré-configurado(s) associado(s) a ela.");
            }
            if (entity.DialogoSet.Any())
            {
                result = true;
                msgList.Add("Operação abortada. A Secretaria não pode ser deletada pois há Diálogo(s) associado(s) a ela.");
            }
            if (result)
            {
                throw new PiBusinessException(msgList);
            }
            return base.CanDeleteAsync(entity);
        }

        #endregion
    }
}