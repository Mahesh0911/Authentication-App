using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Authentication_App.Filters
{
    public class CustomerAuthentication:FilterAttribute,IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated == false)
            {
                filterContext.Result = new RedirectResult("/Account/Login");
            }

        }
    }
}