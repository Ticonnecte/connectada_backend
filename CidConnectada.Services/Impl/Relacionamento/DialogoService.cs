using CidConnectada.Dao.Notificacao;
using CidConnectada.Dao.Relacionamento;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Banners;
using CidConnectada.Entities.Model.Relacionamento;
using CidConnectada.Services.Intf.AWS;
using CidConnectada.Services.Intf.Relacionamento;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Zenite.Pi.Context;
using Zenite.Pi.IoC;
using Zenite.Pi.Services.Impl;

namespace CidConnectada.Services.Impl.Relacionamento
{
    public class DialogoService : CadastroBaseService<Dialogo, DialogoDao, string, int, string>, IDialogoService
    {
        public DialogoService(DialogoDao cadDao, Func<ContextRequest<int, string>> contextFactory)
            : base(cadDao, contextFactory)
        {
        }

        #region CRUD

        public override string GetNomeEntidade(int indexDetail = 0)
        {
            return "Fale com o Prefeito";
        }

        public override object GetValorCampoDescritivoPadrao(Dialogo entity)
        {
            string autor = entity.Cidadao is null ? "Anônimo" : entity.Cidadao.Nome;
            return $"{entity.AssuntoDialogoEnum} de {autor}, em {entity.DhCriacao}";
        }

        protected override Expression<Func<Dialogo, bool>> GetUnicidadeFilter(Dialogo entity)
        {
            double radius = Double.Parse(ApplicationContext.AppSettings["Geography:Location:Radius"]);
            int prazoRepeticao = Int32.Parse(ApplicationContext.AppSettings["Dialogo:PrazoRepeticao"]);

            var query = cadDao.Where(d => d.Secretaria.Key == entity.Secretaria.Key
                && d.Endereco.Coordenadas.Distance(entity.Endereco.Coordenadas) <= radius
                && d.AssuntoDialogoEnum == entity.AssuntoDialogoEnum
                && d.Key != entity.Key);

            if (entity.Cidadao != null)
                query = query.Where(d => d.Cidadao.Key == entity.Cidadao.Key);

            IList<Dialogo> dialogos = query.ToList();

            if (dialogos.Any(dialogo => dialogo.DhCriacao.AddDays(prazoRepeticao) > entity.DhCriacao))
                return e => true;

            return e => false;
        }

        #endregion

        #region Custom

        public async Task<IList<Dialogo>> GetMyDialogos()
        {
            int userKey = ((Usuario)Context.User).Key;
            return await cadDao.Where(d => d.Cidadao.Key == userKey).ToListAsync();
        }

        public async Task<Dialogo> IncluirAsync(Dialogo entity, Delegate upload)
        {
            return await IncluirAsync(entity);
        }

        public async Task AlterarAsync(Dialogo entity, Delegate upload)
        {
            await AlterarAsync(entity);
        }

        public async Task DeleteAsync(Dialogo entity, Delegate deleteS3)
        {
            await ExcluirAsync(entity);
        }

        #endregion
    }
}
