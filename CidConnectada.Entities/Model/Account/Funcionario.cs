using System;
using System.Collections.Generic;
using CidConnectada.Entities.Model.Banners;
using CidConnectada.Entities.Model.Noticias;
using CidConnectada.Entities.Model.Relacionamento;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Account
{
    public class Funcionario : Usuario, IEquatable<Funcionario>
    {

        public ISet<HistoricoDialogo> HistoricoDialogoSet { get; set; }
        public bool Equals(Funcionario other)
        {
            bool result;
            if (ReferenceEquals(other, null))
                result = false;
            else if (ReferenceEquals(other, this))
                result = true;
            else
                result = EntityUtil.EqualsEntity(this, other);

            return result;
        }
    }
}