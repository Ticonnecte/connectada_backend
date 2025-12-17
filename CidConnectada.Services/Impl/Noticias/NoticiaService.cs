using CidConnectada.Dao.Noticias;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Dto;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Entities.Model.Infos;
using CidConnectada.Entities.Model.Noticias;
using CidConnectada.Services.Intf.Account;
using CidConnectada.Services.Intf.AWS;
using CidConnectada.Services.Intf.Noticias;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Zenite.Pi.Context;
using Zenite.Pi.Services.Impl;

namespace CidConnectada.Services.Impl.Noticias
{
    public class NoticiaService : CadastroMasterBaseService<Noticia, NoticiaDao, string, NoticiaImages,HtmlImagesKey, NoticiaImagesDao, int, string>, INoticiaService
    {
        public NoticiaService(NoticiaDao _cadDao, 
            Func<ContextRequest<int, string>> contextFactory,
            NoticiaImagesDao detailDao,
            IUsuarioService usuarioService

            )
              : base(_cadDao, contextFactory, detailDao) 
            {
                UsuarioService = usuarioService;
            }

        #region Dao e Service
        private readonly IUsuarioService UsuarioService;

        #endregion

        #region CRUD

        public override string GetNomeEntidade(int indexDetail = 0)
        {
            return "Notícia";
        }

        public override object GetValorCampoDescritivoPadrao(Noticia entity)
        {
            return $"Nome: {entity.Lead}";
        }

        protected override Expression<Func<Noticia, bool>> GetUnicidadeFilter(Noticia entity)
        {
            return e => (e.Lead == entity.Lead || e.Conteudo == entity.Conteudo) && e.Key != entity.Key;
        }

        #endregion
        
        #region Custom

        public async Task<Noticia> IncluirAsync(Noticia entity, Delegate upload)
        {
            await InsertLog(entity);
            return await IncluirAsync(entity);
        }

        public async Task AlterarAsync(Noticia entity, ISet<NoticiaImages> listEntitiesDetail1, Delegate upload)
        {
            await InsertLog(entity);
            await AlterarAsync(entity, listEntitiesDetail1);
        }

        public async Task DeleteAsync(Noticia entity, Delegate deleteS3)
        {
            await ExcluirAsync(entity);
        }

        private async Task InsertLog(Noticia entity)
        {
            entity.NoticiaLogSet.Add(new NoticiaLog
            {
                NoticiaId = entity.Key,
                Noticia = entity,
                DhUpdate = DateTime.Now,
                Usuario = await UsuarioService.ObterAsync(((Usuario)Context.User).Key)
            });
        }

        public async Task SendedMessageRegister(EnvioNoticia envioNoticia, string zaapId, string messageId)
        {
            envioNoticia.ZaapId = zaapId;
            envioNoticia.MessageId = messageId;
            envioNoticia.DhEnvio = DateTime.Now;
            envioNoticia.StatusEnum = string.IsNullOrEmpty(messageId) ? EnvioMsgStatusEnum.PENDING : EnvioMsgStatusEnum.SENT;

            //TODO: Criar monitoramento de recebimento/leitura de noticias
            //await EnvioNoticiaDao.SendedMessageRegister(envioNoticia);
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

    }
}
