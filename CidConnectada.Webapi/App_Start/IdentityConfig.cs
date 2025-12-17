using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using CidConnectada.Webapi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Zenite.Pi.IoC;

namespace CidConnectada.Webapi
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            string tId = context.Request.Headers.GetValues("TenantId")?.FirstOrDefault();
            int.TryParse(tId, out int tenantId);
            
            var manager = new ApplicationUserManager(new ApplicationUserStore<ApplicationUser>(context.Get<ApplicationDbContext>(), tenantId));
            
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false,
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        public int GetTenantId()
        {
            return ((ApplicationUserStore<ApplicationUser>)Store).TenantKey;
        }
    }

    public class ApplicationUserStore<TUser> : UserStore<TUser>, IUserStore<TUser>
        where TUser : ApplicationUser {
        public ApplicationUserStore(ApplicationDbContext context, int tenantId)
            : base(context)
        {
            TenantKey = tenantId;
        }
        
        public int TenantKey { get; set; }
        
        public override Task CreateAsync(TUser user) {
            if (user == null) {
                throw new ArgumentNullException(nameof(user));
            }

            user.TenantKey = this.TenantKey;
            return base.CreateAsync(user);
        }
        
        public override Task<TUser> FindByEmailAsync(string email) {
            return this.GetUserAggregateAsync(u => u.Email.ToUpper() == email.ToUpper() 
                && u.TenantKey == this.TenantKey);
        }
        
        public override Task<TUser> FindByNameAsync(string userName) {
            return this.GetUserAggregateAsync(u => u.UserName.ToUpper() == userName.ToUpper() 
                && u.TenantKey == this.TenantKey);
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