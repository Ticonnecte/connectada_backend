using CidConnectada.Entities.Model.Identity;
using CidConnectada.Webapi.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;

namespace CidConnectada.Dao.Identity
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnectionString", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.HasDefaultSchema("dbo");
            
            var user = modelBuilder.Entity<ApplicationUser>();
            
            user.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(
                    new IndexAttribute("UserNameIndex") { IsUnique = true, Order = 1}));
            
            user.Property(u => u.TenantKey)
                .HasColumnName("TENANT_ID")
                .IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(
                    new IndexAttribute("UserNameIndex") { IsUnique = true, Order = 2 }));
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        
        protected override DbEntityValidationResult ValidateEntity(
            DbEntityEntry entityEntry, IDictionary<object, object> items) {
            if (entityEntry != null && entityEntry.State == EntityState.Added) {
                var errors = new List<DbValidationError>();
                var user = entityEntry.Entity as ApplicationUser;

                if (user != null) {
                    if (this.Users.Any(u => string.Equals(u.UserName, user.UserName) 
                        && u.TenantKey == user.TenantKey)) {
                        errors.Add(new DbValidationError("User", 
                            string.Format("Username {0} is already taken for AppId {1}", 
                                user.UserName, user.TenantKey)));
                    }

                    if (this.RequireUniqueEmail 
                        && this.Users.Any(u => string.Equals(u.Email, user.Email) 
                            && u.TenantKey == user.TenantKey)) {
                        errors.Add(new DbValidationError("User", 
                            string.Format("Email Address {0} is already taken for AppId {1}", 
                                user.UserName, user.TenantKey)));
                    }
                }
                else {
                    var role = entityEntry.Entity as IdentityRole;

                    if (role != null && this.Roles.Any(r => string.Equals(r.Name, role.Name))) {
                        errors.Add(new DbValidationError("Role", 
                            string.Format("Role {0} already exists", role.Name)));
                    }
                }
                if (errors.Any()) {
                    return new DbEntityValidationResult(entityEntry, errors);
                }
            }

            return new DbEntityValidationResult(entityEntry, new List<DbValidationError>());
        }
    }
}