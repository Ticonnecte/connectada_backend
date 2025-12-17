using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Noticias;
using Microsoft.SqlServer.Management.XEvent;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Utilities.Zlib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Web;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Dao.Noticias
{
    public class NoticiaDao : MultiTenancyDao<Noticia, string, int, int, string>
    {
        public NoticiaDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }

        public override Dictionary<string, ListSortDirection> DefaultOrder => new Dictionary<string, ListSortDirection>
        {
            {
                "Lead", ListSortDirection.Ascending
            }
        };

        protected override int TenantValue
        {
            get => DaoHelper.GetTenantId() ?? base.TenantValue;
        }

        public override string[] DefaultIncludes => new string[3]
        {
            // ATENÇÃO: O problema de lidar com strings, os erros passam, compilam e ninguém vê.

            //ERRO: A specified Include path is not valid. The EntityType 'Castle.Proxies.Noticia' does not declare a navigation property with the name 'NoticiaImageSet'.

            //"NoticiaCategoriaVincSet", "NoticiaLogSet.Usuario", "NoticiaImageSet"
            "NoticiaCategoriaVincSet", "NoticiaLogSet.Usuario", "NoticiaImagesSet"
        };
        
        #region Custom

        public override IQueryable<Noticia> Where(Expression<Func<Noticia, bool>> predicate, string[] includes, string caller = "")
        {
            switch (caller)
            {
                case "SearchPagedAsync":
                    bool isMobile = RequestContext.User == null || ((Usuario)RequestContext.User).AspNetUsers.AspNetUserRolesSet.Any(ur => ur.AspNetRoles.Name == "CIDADAO");
                    predicate = predicate.And(n => !isMobile || (isMobile && n.Ativa.HasValue && n.Ativa.Value));
                    break;
            }
            return base.Where(predicate, includes, caller);
        }

        #endregion
    }
}
