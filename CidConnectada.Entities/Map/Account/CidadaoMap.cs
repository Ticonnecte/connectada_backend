using CidConnectada.Entities.Model.Account;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Account
{
    public class CidadaoMap : EntityBaseMap<Cidadao, int>
    {
        public CidadaoMap()
        {
            ToTable("CIDADAO");

            HasOptional(e => e.Bairro).WithMany(e => e.CidadaoSet).Map(e => e.MapKey("BAIRRO_ID"));
        }
    }
}