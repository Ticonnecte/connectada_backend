using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using CidConnectada.Entities.Model.Infos;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Dao.Infos { 
     
    public class InfoImagesDao : BaseDao<InfoImages, HtmlImagesKey, int, string>
    {
        public InfoImagesDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }

    }
}

