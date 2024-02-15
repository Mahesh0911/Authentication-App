using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Authentication_App.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email can't be blank"),
        EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password can't be blank")]
        public string Password { get; set; }
    }
}