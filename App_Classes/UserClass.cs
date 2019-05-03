using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_CircloidTemplate.App_Classes
{
    public class UserClass
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        //[Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        //public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string PasswordQuestion { get; set; }
        public string  PasswordAnswer{ get; set; }

        

    }
}