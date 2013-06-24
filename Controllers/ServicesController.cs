using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FlyNymph.Controllers
{
    /// <summary>
    /// Колнтроллер обработки front-end-сервисов
    /// </summary>
    public class ServicesController : Controller
    {
        //
        // GET: /Services/
        [HttpPost]
        public ActionResult Loginza(string token)
        {
            LoginzaProvider.Core.LoginzaProvider prov = new LoginzaProvider.Core.LoginzaProvider(1001,"SecurityKey",true);
            LoginzaProvider.Core.AuthenticationData data_user = prov.GetAuthenticationData(token);
            return View(data_user);
        }

    }
}
