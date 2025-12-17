using System;
using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Account;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Account
{
    public class VerificacaoContaMap : EntityBaseMap<VerificacaoConta, Guid>
    {
        public VerificacaoContaMap()
        {
            ToTable("VERIFICACAO_CONTA");

            Property(e => e.Key)
                .HasColumnName("ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.Codigo)
                .HasColumnName("CODIGO")
                .HasMaxLength(6);

            Property(e => e.DataExpiracaoUtc)
                .HasColumnName("DATA_EXPIRACAO_UTC");

            HasOptional(e => e.Usuario).WithOptionalDependent(e => e.VerificacaoConta).Map(e => e.MapKey("ID_USUARIO"));
        }
    }
}