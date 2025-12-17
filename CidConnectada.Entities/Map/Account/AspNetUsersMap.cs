using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Account;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Account
{
    public class AspNetUsersMap : EntityBaseMap<AspNetUsers, string>
    {
        public AspNetUsersMap()
        {
            ToTable("AspNetUsers");
            Property(e => e.Key)
                .HasColumnName("Id").IsRequired()
                .HasMaxLength(128)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.Username).HasColumnName("UserName")
                .IsRequired()
                .HasMaxLength(256);

            Property(e => e.Email).HasColumnName("Email")
                .HasMaxLength(256);

            Property(e => e.Emailconfirmed).HasColumnName("EmailConfirmed")
                .IsRequired();

            Property(e => e.Passwordhash).HasColumnName("PasswordHash")
                .HasMaxLength(512);

            Property(e => e.Securitystamp).HasColumnName("SecurityStamp")
                .HasMaxLength(256);

            Property(e => e.PhoneNumber).HasColumnName("PhoneNumber")
                .HasMaxLength(15);

            Property(e => e.Phonenumberconfirmed).HasColumnName("PhoneNumberConfirmed")
                .IsRequired();

            Property(e => e.Twofactorenabled).HasColumnName("TwoFactorEnabled")
                .IsRequired();

            Property(e => e.Lockoutenddateutc).HasColumnName("LockoutEndDateUtc");

            Property(e => e.Lockoutenabled).HasColumnName("LockoutEnabled")
                .IsRequired();

            Property(e => e.Accessfailedcount).HasColumnName("AccessFailedCount")
                .IsRequired();
            
            Property(e => e.TenantKey)
                .HasColumnName("TENANT_ID")
                .IsRequired();
        }
    }
}