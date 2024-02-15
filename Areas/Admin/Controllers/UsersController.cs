using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Authentication_App.Filters;
using Authentication_App.Identity;
using Microsoft.AspNet.Identity;

namespace Authentication_App.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        // GET: Admin/Users
        [AdminAuthentication]
        public ActionResult Users()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var users = db.Users.ToList();
            return View(users);

        }

        [AdminAuthentication]
        public ActionResult UserDetails(string email)
        {

            if (email == null)
            {
                return RedirectToAction("Users");
            }
            var appDbContext = new ApplicationDbContext();
            var userStore = new ApplicationUserStore(appDbContext);
            var userManager = new ApplicationUserManager(userStore);
            var user = userManager.FindByEmail(email);
            return View(user);
        }

        [AdminAuthentication]
        public ActionResult Delete(string email)
        {
            if (email == null)
            {
                return RedirectToAction("Users");
            }

            var appDbContext = new ApplicationDbContext();
            var userStore = new ApplicationUserStore(appDbContext);
            var userManager = new ApplicationUserManager(userStore);
            var user = userManager.FindByEmail(email);

            userManager.Delete(user);

            return RedirectToAction("UserDetails");
        }

        [AdminAuthentication]
        public ActionResult Deactivate(string email)
        {
            if (email == null)
            {
                return RedirectToAction("Users");
            }
            ApplicationDbContext db = new ApplicationDbContext();
            var user = db.Users.Where(u => u.Email == email).FirstOrDefault();
            user.isDisabled = true;
            db.SaveChanges();
            return RedirectToAction("UserDetails");
        }

        [AdminAuthentication]
        public ActionResult Activate(string email)
        {
            if (email == null)
            {
                return RedirectToAction("Users");
            }
            ApplicationDbContext db = new ApplicationDbContext();
            var user = db.Users.Where(u => u.Email == email).FirstOrDefault();
            user.isDisabled = false;
            db.SaveChanges();
            return RedirectToAction("UserDetails");
        }
    }
}