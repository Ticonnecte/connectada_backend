using System;
using System.Linq;
using System.Threading.Tasks;
using CidConnectada.Entities.Model.Identity;
using CidConnectada.Webapi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CidConnectada.Dao.Identity
{
    public class ApplicationUserStore<TUser> : UserStore<TUser>, IUserStore<TUser>
        where TUser : ApplicationUser {
        public ApplicationUserStore(ApplicationDbContext context, int tenantId)
            : base(context)
        {
            TenantKey = tenantId;
            AppDbContext = context;
        }
        
        private int TenantKey { get; set; }

        private ApplicationDbContext AppDbContext { get; set; }
        
        public override Task CreateAsync(TUser user) 
        {
            if (user == null) {
                throw new ArgumentNullException(nameof(user));
            }

            if (user.TenantKey == 0)
                user.TenantKey = this.TenantKey;
            
            return base.CreateAsync(user);
        }
        
        public override Task<TUser> FindByEmailAsync(string email) 
        {
            return this.GetUserAggregateAsync(u => u.Email.ToUpper() == email.ToUpper() 
                && (u.Roles.Any(ur => AppDbContext.Roles.Any(r => r.Id == ur.RoleId && r.Name == "SA") || 
                    u.TenantKey == this.TenantKey)));
        }
        
        public override Task<TUser> FindByNameAsync(string userName) 
        {
            return this.GetUserAggregateAsync(u => u.UserName.ToUpper() == userName.ToUpper() 
                && (u.Roles.Any(ur => AppDbContext.Roles.Any(r => r.Id == ur.RoleId && r.Name == "SA") || 
                    u.TenantKey == this.TenantKey)));
        }
    }

    //public class ApplicationRoleManager : RoleManager<IdentityRole>
    //{
    //    public ApplicationRoleManager(IRoleStore<IdentityRole, string> roleStore)
    //        : base(roleStore)
    //    {
    //    }

    //    public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
    //    {
    //        var appRoleManager = new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));

    //        return appRoleManager;
    //    }
    //}
}