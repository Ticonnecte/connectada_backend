using CidConnectada.Entities.Model.Comunicacao;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Comunicacao
{
    public class EnqueteRespostaDao : BaseDao<EnqueteResposta, int, int, string>
    {
        public EnqueteRespostaDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
        public override Dictionary<string, ListSortDirection> DefaultOrder
        {
            get => new Dictionary<string, ListSortDirection>
            {
                { "EnqueteOpcao.EnqueteId", ListSortDirection.Ascending },
                { "EnqueteOpcao.OpcaoIdx", ListSortDirection.Ascending }
            };
        }

        public override string[] DefaultIncludes
        {
            get => new string[2]
            {
                "EnqueteOpcao.Enquete", "Usuario"
            };
        }
    }
}