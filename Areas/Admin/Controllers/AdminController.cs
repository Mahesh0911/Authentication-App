using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Authentication_App.Filters;

namespace Authentication_App.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin/Admin
        [AdminAuthentication]
        public ActionResult Admin()
        {
            return View();
        }
    }
}