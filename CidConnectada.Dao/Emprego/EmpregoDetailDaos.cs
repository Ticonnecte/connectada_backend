using CidConnectada.Entities.Model.Emprego;
using FuzzySharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Emprego
{
    public abstract class EmpregoDetailGenericDao<TEntity> : BaseDao<TEntity, int, int, string>
        where TEntity : EmpregoDetail
    {
        public EmpregoDetailGenericDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }

        public override Dictionary<string, ListSortDirection> DefaultOrder
        {
            get => new Dictionary<string, ListSortDirection>
            {
                { "Nome", ListSortDirection.Ascending }
            };
        }

        public async Task<IList<string>> FuzzySearch(string termo, int limite)
        {
            //TODO: Usar caching
            termo = termo ?? string.Empty;
            IList<string> result = (await AllAsync()).Select(d => d.Nome).ToList();
            result = result.Select(nome => new
            {
                Texto = nome,
                Score = Fuzz.Ratio(termo.ToLowerInvariant(), nome.ToLowerInvariant())
            }).OrderByDescending(x => x.Score)
                .ThenBy(x => x.Texto)
                .Take(limite > 30 ? 30 : limite)
                .Select(x => x.Texto)
                .ToList();

            return result;
        }
    }

    public class EmpregoDetailDao : EmpregoDetailGenericDao<EmpregoDetail>
    {
        public EmpregoDetailDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
    }

    public class CompetenciaDao : EmpregoDetailGenericDao<Competencia>
    {
        public CompetenciaDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
    }

    public class FuncaoDao : EmpregoDetailGenericDao<Funcao>
    {
        public FuncaoDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
    }

    public class HabilidadeDao : EmpregoDetailGenericDao<Habilidade>
    {
        public HabilidadeDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
    }

    public class SetorMercadoDao : EmpregoDetailGenericDao<SetorMercado>
    {
        public SetorMercadoDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
    }
}