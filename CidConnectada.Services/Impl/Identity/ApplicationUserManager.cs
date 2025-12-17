using CidConnectada.Dao.Identity;
using CidConnectada.Entities.Model.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Linq;
using Zenite.Pi.Context;
using Zenite.Pi.IoC;

namespace CidConnectada.Services.Impl.Identity
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

            int tenantId = 0;
            
          if (context.Request.Headers.TryGetValue("TenantId", out var tenantHeaders) 
                && Int32.TryParse(tenantHeaders.FirstOrDefault(), out tenantId))
            {
                ContextRequest<int, string> ctxRequest = ApplicationContext.Resolve<ContextRequest<int, string>>();
                if (ctxRequest != null)
                {
                    ctxRequest.CacheRequest["TenantId"] = tenantId;
                }
            }

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