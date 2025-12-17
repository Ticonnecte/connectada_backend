using CidConnectada.Entities.Model.Emprego;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Emprego
{
    public class CVExperienciaDao : BaseDao<CVExperiencia, CVExperienciaKey, int, string>
    {
        public CVExperienciaDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
        public override Dictionary<string, ListSortDirection> DefaultOrder => new Dictionary<string, ListSortDirection>
        {
            { "CVId", ListSortDirection.Ascending }
        };

        public override string[] DefaultIncludes => new string[2]
        {
            "CurriculumVitae", "Funcao"
        };
    }
}