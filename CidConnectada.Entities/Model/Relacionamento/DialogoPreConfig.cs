using System;
using System.Collections.Generic;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Entities.Model.Local;
using CidConnectada.Entities.Model.Organograma;
using Zenite.Pi.Entities;
using Zenite.Pi.Entities.Model.MultiTenancy;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Relacionamento
{
    public class DialogoPreConfig : MultiTenancy<int, int>, IEquatable<DialogoPreConfig>
    {
        public string Nome { get; set; }
        public string IconeNome { get; set; }
        public string TituloPadrao { get; set; }
        public DialogoAssuntoEnum AssuntoDialogoEnum { get; set; }
        public Secretaria Secretaria { get; set; }
        public bool Equals(DialogoPreConfig other)
        {
            bool result;
            if (ReferenceEquals(other, null))
            {
                result = false;
            }
            else if (ReferenceEquals(other, this))
            {
                result = true;
            }
            else
            {
                result = EntityUtil.EqualsEntity(this, other);
            }

            return result;
        }
    }
}