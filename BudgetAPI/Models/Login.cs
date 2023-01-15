using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetAPI.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Email is not empty!")]
        [EmailAddress(ErrorMessage = "Email is not valid!")]
        [MaxLength(30)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is not empty!")]
        [MinLength(3)]
        public string Password { get; set; }
    }
}
