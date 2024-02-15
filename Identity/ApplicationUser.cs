using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Authentication_App.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string ProfilePhoto { get; set; }
        public bool isDisabled { get; set; }
    }
}