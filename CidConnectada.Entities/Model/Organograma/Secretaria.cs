using System;
using System.Collections.Generic;
using CidConnectada.Entities.Model.Relacionamento;
using Zenite.Pi.Entities.Model.MultiTenancy;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Organograma
{
    public class Secretaria : MultiTenancy<string, int>, IEquatable<Secretaria>
    {
        public override string Key { get; set; }
        public string Nome { get; set; }
        public string NomeSecretario { get; set; }
        public string IconeNome { get; set; }
        public byte? OrdemHome { get; set; }
        public bool IsActive { get; set; }
        public Prefeitura Prefeitura { get; set; }
        public ISet<SecretariaMenu> SecretariaMenuSet { get; set; } = new HashSet<SecretariaMenu>();
        public ISet<Dialogo> DialogoSet { get; set; } = new HashSet<Dialogo>();
        public ISet<DialogoPreConfig> DialogoPreConfigSet { get; set; } = new HashSet<DialogoPreConfig>();

        public bool Equals(Secretaria other)
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