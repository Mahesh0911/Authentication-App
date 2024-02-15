using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Security;
using Authentication_App.Identity;
using Authentication_App.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Authentication_App.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Register()
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
            var rvm = new RegisterViewModel();
            rvm.DateOfBirth = DateTime.Now;
            rvm.Country = "India";
            rvm.Address = "Pune";
            rvm.PhoneNumber = null;
            return View(rvm);
        }

        //post request
        [HttpPost]
        public ActionResult Register(RegisterViewModel rvm)
        {
            var file = Request.Files["ProfilePhoto"];
            if (file != null && file.ContentLength>0 )
            {
                    var imgBytes = new Byte[file.ContentLength];
                    file.InputStream.Read(imgBytes, 0, file.ContentLength);
                    var base64String = Convert.ToBase64String(imgBytes, 0, imgBytes.Length);
                    rvm.ProfilePhoto = base64String;

            }
            else
            {
                var path = HostingEnvironment.MapPath("~/Images/UserPhoto.jpg");
                ViewBag.Message = path;
                var imgBytes = System.IO.File.ReadAllBytes(path);
                var base64String = Convert.ToBase64String(imgBytes, 0, imgBytes.Length);
                rvm.ProfilePhoto = base64String;
            }



            if (ModelState.IsValid)
            {
                //register
                var appDbContext = new ApplicationDbContext();
                var userStore = new ApplicationUserStore(appDbContext);
                ApplicationUserManager userManager = new ApplicationUserManager(userStore);

                var passwordHash = Crypto.HashPassword(rvm.PasswordHash);

                var userExisting = userManager.FindByEmail(rvm.Email);
                if (userExisting != null)
                {
                    ViewBag.Message = "You can login...";
                    ModelState.AddModelError("Error", "Account with specified email present.");
                    return View(rvm);
                }


                rvm.isDisabled = false;


                ApplicationUser user = new ApplicationUser()
                {
                    UserName = rvm.UserName,
                    Email = rvm.Email,
                    PasswordHash = passwordHash,
                    isDisabled = rvm.isDisabled,
                    Address = rvm.Address,
                    Country = rvm.Country,
                    DateOfBirth = rvm.DateOfBirth,
                    PhoneNumber = rvm.PhoneNumber,
                    ProfilePhoto = rvm.ProfilePhoto
                };

                IdentityResult result = userManager.Create(user);
                if (result.Succeeded)
                {
                    //role
                    userManager.AddToRole(user.Id, "Customer");

                    //login
                    var authenticationManager = HttpContext.GetOwinContext().Authentication;
                    var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    authenticationManager.SignIn(new AuthenticationProperties(), userIdentity);
                    ViewBag.Message = "Registration successful";

                    return RedirectToAction("CustomerDashboard", "Customer", new { area = "Customer" });

                }
                else
                {
                    ViewBag.Message = "Registration Unsuccessful !";
                    ModelState.AddModelError("MyError", "Invalid data");
                    return View(rvm);
                }
            }
            
            
            @ViewBag.Message = "Registration Unsuccessful !";
            ModelState.AddModelError("MyError", "Invalid data");
            return View(rvm);
        }

      

        public ActionResult Login()
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

        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {

            if (ModelState.IsValid)
            {
                var appDbContext = new ApplicationDbContext();
                var userStore = new ApplicationUserStore(appDbContext);
                var userManager = new ApplicationUserManager(userStore);

                var user = userManager.FindByEmail(loginViewModel.Email);

                if (user != null)
                {
                    if (user.isDisabled == true)
                    {
                        @ViewBag.Message = "Account is disabled";
                        return View();
                    }
                    //login
                    var authenticationManager = HttpContext.GetOwinContext().Authentication;
                    var result = userManager.PasswordHasher.VerifyHashedPassword(user.PasswordHash, loginViewModel.Password);
                    if (result == PasswordVerificationResult.Failed)
                    {
                        @ViewBag.Message = "Invalid Password";
                        return View();
                    }
                    var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

                    authenticationManager.SignIn(new AuthenticationProperties(), userIdentity);

                    if (userManager.IsInRole(user.Id, "Admin"))
                    {
                        return RedirectToAction("admin", "Admin", new { area = "Admin" });
                    }
                    else if (userManager.IsInRole(user.Id, "Customer"))
                    {
                        return RedirectToAction("CustomerDashboard", "Customer", new { area = "Customer" });
                    }
                }
                else
                {
                    ModelState.AddModelError("My Error", "Invalid data");
                }
            }
            ViewBag.Message = "Invalid data";
            return View();

        }

        public ActionResult Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            var authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("index", "home");
        }



    }

}