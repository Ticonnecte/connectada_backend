using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using CidConnectada.Entities.Model.Infos;
using CidConnectada.Entities.Model.Noticias;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Dao.Noticias { 
     
    public class NoticiaImagesDao : BaseDao<NoticiaImages, HtmlImagesKey, int, string>
    {
        public NoticiaImagesDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }

    }
}

