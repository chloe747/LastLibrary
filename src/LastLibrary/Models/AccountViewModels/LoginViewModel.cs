using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LastLibrary.Middleware.Validators;

namespace LastLibrary.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        [UsernameOrEmail]
        [Display(Name = "Username or Email")]
        public string UsernameOrEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
