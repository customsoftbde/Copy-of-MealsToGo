using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MealsToGo.ViewModels
{
    public class LoginRegisterViewModel
    {
        [Required]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }

      
    }
}