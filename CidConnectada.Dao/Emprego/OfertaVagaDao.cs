using CidConnectada.Entities.Model.Emprego;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Emprego
{
    public class OfertaVagaDao : MultiTenancyDao<OfertaVaga, long, int, int, string>
    {
        public OfertaVagaDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }

        public override Dictionary<string, ListSortDirection> DefaultOrder => new Dictionary<string, ListSortDirection>
        {
            { "StatusEnum", ListSortDirection.Ascending },
            { "DhCriacao", ListSortDirection.Descending }
        };

        protected override int TenantValue
        {
            get => DaoHelper.GetTenantId() ?? base.TenantValue;
        }

        public override string[] DefaultIncludes =>
            RequestContext.CacheRequest.ContainsKey("IsGetOne") ?
                new string[7]
                {
                    "Prefeitura", "FaixaSalarial", "OfertaVagaCompetenciaSet.Competencia",
                    "OfertaVagaHabilidadeSet.Habilidade", "Funcao", "SetorMercado", "Endereco.Cidade.Estado"
                }
                : new string[4] { "Prefeitura", "FaixaSalarial", "Funcao", "Empregador" };

        public override async Task<OfertaVaga> FindByKeyAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            if (!RequestContext.CacheRequest.TryGetValue("IsGetOne", out object _))
                RequestContext.CacheRequest.Add("IsGetOne", true);
            return await base.FindByKeyAsync(cancellationToken, keyValues);
        }

        public override OfertaVaga FindByKey(params object[] keyValues)
        {
            if (!RequestContext.CacheRequest.TryGetValue("IsGetOne", out object _))
                RequestContext.CacheRequest.Add("IsGetOne", true);
            return base.FindByKey(keyValues);
        }
    }
}