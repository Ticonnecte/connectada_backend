using CidConnectada.Entities.Model.Organograma;
using System;
using Zenite.Pi.Context;
using Zenite.Pi.Dao.Impl;

namespace CidConnectada.Dao.Organograma
{
    public class SecretariaMenuDao : BaseDao<SecretariaMenu, SecretariaMenuKey, int, string>
    {
        public SecretariaMenuDao(Func<ContextRequest<int, string>> contextFactory) : base(contextFactory)
        {
        }
        public override string[] DefaultIncludes
        {
            get => new string[2] { "Secretaria", "RotaInterna" };
        }
    }
}