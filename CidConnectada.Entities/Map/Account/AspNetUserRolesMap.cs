using CidConnectada.Entities.Model.Account;
using System.ComponentModel.DataAnnotations.Schema;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Account
{
    public class AspNetUserRolesMap : EntityBaseMap<AspNetUserRoles, AspNetUserRolesKey>
    {
        public AspNetUserRolesMap()
            : base()
        {
            ToTable("AspNetUserRoles");
            Property(e => e.UserId)
                .HasColumnName("UserId").IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(e => e.RoleId)
                .HasColumnName("RoleId").IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            HasRequired(e => e.AspNetUsers).WithMany(e => e.AspNetUserRolesSet).HasForeignKey(e => e.UserId);
            HasRequired(e => e.AspNetRoles).WithMany(e => e.AspNetUserRolesSet).HasForeignKey(e => e.RoleId);

        }

        protected override void DefineHasKey()
        {
            HasKey(entity => new { entity.UserId, entity.RoleId });
        }
    }
}
