using CidConnectada.Entities.Model.Emprego;
using CidConnectada.Entities.Model.Relacionamento;
using CidConnectada.Entities.Model.Saude;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using Zenite.Pi.Entities;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Local
{
    public class Endereco : BaseEntity<long>, IEquatable<Endereco>
    {
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public DbGeography Coordenadas { get; set; }
        public string GoogleMapsPlaceId { get; set; }
        public string EnderecoCompleto => String.Join(", ", new[]
        {
            Rua, Numero, Bairro, Cidade?.Nome + " - " + Cidade?.Estado?.Sigla, Cep
        }.Where(s => !String.IsNullOrWhiteSpace(s)));
        public Cidade Cidade { get; set; }
        public ISet<UnidadeBasicaSaude> UBSSet { get; set; } = new HashSet <UnidadeBasicaSaude>();
        public ISet<OfertaVaga> OfertaVagaSet { get; set; } = new HashSet<OfertaVaga>();
        public ISet<Dialogo> DialogoSet { get; set; } = new HashSet<Dialogo>();
        public ISet<Comercios.Comercio> ComercioSet { get; set; } = new HashSet<Comercios.Comercio>();

        public bool Equals(Endereco other)
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
