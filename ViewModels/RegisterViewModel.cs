using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace Authentication_App.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Username can't be blank")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password can't be blank")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Confirm Password can't be blank"),
            Compare("PasswordHash", ErrorMessage = "Password and Confirm Password must match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email can't be blank"),
        EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Number can't be blank")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number format. It must have exactly 10 digits.")]
        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string ProfilePhoto { get; set; }

        public string Address { get; set; }

        public bool isDisabled { get; set; }
        public string Country { get; set; }
    }
}