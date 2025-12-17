using CidConnectada.Entities.Model.Emprego;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Emprego
{
    public class OfertaVagaHabilidadeDao : BaseDao<OfertaVagaHabilidade, OfertaVagaHabilidadeKey, int, string>
    {
        public OfertaVagaHabilidadeDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
        public override Dictionary<string, ListSortDirection> DefaultOrder => new Dictionary<string, ListSortDirection>
        {
            { "OfertaVaga.StatusEnum", ListSortDirection.Ascending },
            { "OfertaVaga.DhCriacao", ListSortDirection.Descending }
        };

        public override string[] DefaultIncludes => new string[2]
        {
            "OfertaVaga", "Habilidade"
        };
    }
}