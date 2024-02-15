using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Filters;
using System.Web.Mvc;

namespace Authentication_App.Filters
{
    public class AdminAuthentication:FilterAttribute,IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if (!(filterContext.HttpContext.User.Identity.IsAuthenticated && filterContext.HttpContext.User.IsInRole("Admin")))
            {
                //filterContext.Result = new HttpUnauthorizedResult();
                filterContext.Result = new RedirectResult("/Account/Login");
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            //throw new NotImplementedException();


        }
    }
}