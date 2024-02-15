using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Authentication_App.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("admin", "Admin", new { area = "Admin" });
                }
                else if (User.IsInRole("Customer"))
                {
                    return RedirectToAction("CustomerDashboard", "Customer", new { area = "Customer" });
                }
            }
            return View();
        }
    }
}