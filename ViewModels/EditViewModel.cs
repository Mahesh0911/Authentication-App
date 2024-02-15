using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Authentication_App.Filters;

namespace Authentication_App.ViewModels
{
    public class EditViewModel
    {
        [Required(ErrorMessage = "User name can't be blank")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email"),
        Required(ErrorMessage = "Email can't be blank")]
        public string Email { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number format. It must have exactly 10 digits."),]
        public string  PhoneNumber{ get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address{ get; set; }


        public string ProfilePhoto { get; set; }
        public string Country{ get; set; }

    }
}