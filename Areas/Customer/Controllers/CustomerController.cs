using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Authentication_App.Identity;
using Authentication_App.Filters;
using Authentication_App.ViewModels;

namespace Authentication_App.Areas.Customer.Controllers
{
    public class CustomerController : Controller
    {

        // GET: Customer/Customer
        [CustomerAuthentication]
        public ActionResult CustomerDashboard()
        {
            return View();
            
        }

       

        [CustomerAuthentication]
        public ActionResult EditProfile(string name)
        {

            if (name == null)
            {
                return RedirectToAction("CustomerDashboard");
            }

            name = User.Identity.Name;
            var appDbContext = new ApplicationDbContext();
            var userStore = new ApplicationUserStore(appDbContext);
            var userManager = new ApplicationUserManager(userStore);
            var user = userManager.FindByName(name);

            if (user == null)
            {
                // Handle the case where the user is not found
                return RedirectToAction("CustomerDashboard");
            }

            EditViewModel evm = new EditViewModel();
            evm.UserName = user.UserName;
            evm.Email = user.Email;
            evm.PhoneNumber = user.PhoneNumber;
            evm.Address = user.Address;
            evm.Country = user.Country;
            evm.ProfilePhoto = user.ProfilePhoto;

            // Check if user.DateOfBirth is not null before casting
            if (user.DateOfBirth != null)
            {
                evm.DateOfBirth = (DateTime)user.DateOfBirth;
            }

            return View(evm);
        }


        [HttpPost]
        [CustomerAuthentication]
        public ActionResult EditProfile(EditViewModel evm)
        {
            

            var appDbContext = new ApplicationDbContext();
            var userStore = new ApplicationUserStore(appDbContext);
            var userManager = new ApplicationUserManager(userStore);
            var user = userManager.FindByName(User.Identity.Name);
            
            if(!ModelState.IsValid)
            {
                ViewBag.Message = "Error while updating profile.";
                return View(evm);
            }
            
            var userExisting = userManager.FindByEmail(evm.Email);
            
            if(userExisting != null && userExisting.Id != user.Id)
            {
                ViewBag.Message = "Email already exists.";
                return View(evm);
            }

            var file = Request.Files["ProfilePhoto"];
            if (file != null && file.ContentLength > 0)
            {
                var imgBytes = new Byte[file.ContentLength];
                file.InputStream.Read(imgBytes, 0, file.ContentLength);
                var base64String = Convert.ToBase64String(imgBytes, 0, imgBytes.Length);
                evm.ProfilePhoto = base64String;

            }
            user.ProfilePhoto = evm.ProfilePhoto;
            user.UserName = evm.UserName;
            user.Email = evm.Email;
            user.PhoneNumber = evm.PhoneNumber;
            user.Address = evm.Address;
            user.Country = evm.Country;
            user.DateOfBirth = (DateTime?)evm.DateOfBirth ;
            var result = userManager.Update(user);

            
            if (result.Succeeded)
            {
                var newIdentity = new ClaimsIdentity(User.Identity);
                newIdentity.RemoveClaim(newIdentity.FindFirst(ClaimTypes.Name));
                newIdentity.AddClaim(new Claim(ClaimTypes.Name, evm.UserName));
                var authenticationManager = HttpContext.GetOwinContext().Authentication;
                authenticationManager.SignIn(newIdentity);

                ViewBag.Message = "Profile updated successfully!";
                return RedirectToAction("CustomerDashboard");
            }
            else
            {
                ViewBag.Message = "Error while updating profile.";
            }
            return View(evm);
        }

        [CustomerAuthentication]
        public ActionResult DisplayProfile(string name)
        {

            var appDbContext = new ApplicationDbContext();
            var userStore = new ApplicationUserStore(appDbContext);
            var userManager = new ApplicationUserManager(userStore);
            var user = userManager.FindByName(name);
            if (user == null)
            {
                ViewBag.Message = "User not found.";
                return RedirectToAction("CustomerDashboard", "Customer");
            }
            return View(user);

        }

    }
}