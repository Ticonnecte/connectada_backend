using CidConnectada.Entities.Model.Account;
using System.ComponentModel.DataAnnotations.Schema;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Account
{
    public class AspNetRolesMap : EntityBaseMap<AspNetRoles, string>
    {
        public AspNetRolesMap()
            : base()
        {
            ToTable("AspNetRoles");
            Property(e => e.Key).HasColumnName("Id")
                .IsRequired()
                .HasMaxLength(128)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(e => e.Name).HasColumnName("Name")
                .IsRequired()
                .HasMaxLength(256);
            Property(e => e.Description).HasColumnName("DESCRIPTION")
                .HasMaxLength(200);
            Property(e => e.Permissions).HasColumnName("PERMISSIONS")
                .HasMaxLength(4000)
                .IsRequired();
        }
    }
}
