using System.Web.Mvc;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Services.Intf.Account;
using Zenite.Pi.Entities.Model.Account;
using Zenite.Pi.IoC;
using Zenite.Pi.Web;

namespace CidConnectada.Webapi.Controllers
{
    public class HomeController : PiBaseController<int, string>
    {
        #region Services

        protected IUsuarioService accountService => ApplicationContext.Resolve<IUsuarioService>();

        #endregion

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }

        protected override IHieUser<string> GetHieUser()
        {
            Usuario result = null;
            if (Request.IsAuthenticated && IdentityUser != null)
                result = (Usuario)accountService.ObterUser(IdentityUser.Id);
            return result;
        }
    }
}