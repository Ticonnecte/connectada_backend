using CidConnectada.Entities.Model.Emprego;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Emprego
{
    public class CurriculumVitaeDao : BaseDao<CurriculumVitae, int, int, string>
    {
        public CurriculumVitaeDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }

        public override Dictionary<string, ListSortDirection> DefaultOrder => new Dictionary<string, ListSortDirection>
        {
            { "Key", ListSortDirection.Ascending }
        };

        public override string[] DefaultIncludes => new string[6]
        {
            "Cidadao", "Funcao", "SetorMercado", "CVExperienciaSet.Funcao", "CVHabilidadeSet.Habilidade", "CVCompetenciaSet.Competencia"
        };
    }
}