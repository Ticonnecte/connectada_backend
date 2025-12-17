using System.Collections.Generic;
using Zenite.Pi.Entities;

namespace CidConnectada.Entities.Model.Comercios
{
    public class CategoriaTipoComercio : BaseEntity<int>
    {
        public override int Key { get; set; }
        public string Nome { get; set; }
        public TipoComercio TipoComercio { get; set; }
        public ISet<ComercioCategoriaVinculo> ComercioCategoriaVinculoSet { get; set; } = new HashSet<ComercioCategoriaVinculo>();
    }
}
