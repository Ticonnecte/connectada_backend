using System;
using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Account;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Account
{
    public class RefreshTokenMap : EntityBaseMap<RefreshToken, Guid>
    {
        public RefreshTokenMap()
        {
            ToTable("REFRESH_TOKEN");

            Property(e => e.Key)
                .IsRequired()
                .HasColumnName("ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.UserAgent)
                .HasColumnName("USER_AGENT")
                .HasMaxLength(255);

            Property(e => e.IssuedUtc)
                .HasColumnName("ISSUED_UTC");

            Property(e => e.ExpiresUtc)
                .HasColumnName("EXPIRES_UTC");

            Property(e => e.ProtectedTicket)
                .HasColumnName("PROTECTED_TICKET")
                .HasMaxLength(4000);

            HasRequired(e => e.User).WithMany(e => e.RefreshTokenSet).Map(e => e.MapKey("ID_USER"));
            HasRequired(e => e.Device).WithMany(e => e.RefreshTokenSet).Map(e => e.MapKey("ID_DEVICE"));
        }
    }
}