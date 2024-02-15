using Authentication_App.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(Authentication_App.Startup))]

namespace Authentication_App
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions() { AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie, LoginPath = new PathString("/Account/Login") });
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            this.CreateRolesAndUsers();
        }

        private void CreateRolesAndUsers()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var appDbContext = new Identity.ApplicationDbContext();
            var appUserStore = new Identity.ApplicationUserStore(appDbContext);
            var userManager = new UserManager<ApplicationUser>(appUserStore);

            //create admin role
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);



            }
            //create admin user
            if (userManager.FindByName("admin") == null)
            {

                var user = new ApplicationUser();
                user.UserName = "admin";
                user.Email = "admin@gmail.com";
                string userPassword = "admin@123";
                var result = userManager.Create(user, userPassword);
                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                }

            }

            if (!roleManager.RoleExists("Customer"))
            {
                var role = new IdentityRole();
                role.Name = "Customer";
                roleManager.Create(role);
            }

        }
    }
}
