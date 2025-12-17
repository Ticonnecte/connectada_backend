using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Zenite.Pi.Entities;

namespace CidConnectada.Entities.Model.Comercios
{
    public class ComercioCategoriaVinculo: BaseEntity<ComercioCategoriaVinculoKey>
    {
        [IgnoreMap]
        public override ComercioCategoriaVinculoKey Key
        {
            get => new ComercioCategoriaVinculoKey { ComericoId = ComericoId, CategoriaId = CategoriaId };
        }

        [Required]
        public string ComericoId { get; set; }
        [Required]
        public int CategoriaId { get; set; }
        public Comercios.Comercio Comercio { get; set; }

        public CategoriaTipoComercio Categoria { get; set; }
    }
}
