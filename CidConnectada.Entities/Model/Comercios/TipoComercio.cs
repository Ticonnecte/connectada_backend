using System.Collections.Generic;
using Zenite.Pi.Entities.Model.MultiTenancy;

namespace CidConnectada.Entities.Model.Comercios
{
    public class TipoComercio : MultiTenancy<int, int>
    {
        public string Nome { get; set; }
        public byte? OrdemHome { get; set; }
        public string IconeNome { get; set; }
        public bool IsActive { get; set; }

        public ISet<Comercios.Comercio> ComercioSet { get; set; } = new HashSet<Comercios.Comercio>();
        public ISet<CategoriaTipoComercio> CategoriaTipoComercioSet { get; set; } = new HashSet<CategoriaTipoComercio>();
    }
}
