using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Organograma;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Organograma
{
    public class PrefeituraDao : BaseDao<Prefeitura, int, int, string>
    {
        public PrefeituraDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
        public override Dictionary<string, ListSortDirection> DefaultOrder => new Dictionary<string, ListSortDirection>
        {
            { "Nome", ListSortDirection.Ascending }
        };

        public override string[] DefaultIncludes { get => new string[1] { "Endereco.Cidade.Estado" }; }

        //public override Prefeitura Add(Prefeitura entity)
        //{
        //    entity = base.Add(entity);
        //    Context.SaveChanges();
        //    return entity;
        //}
    }
}