using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetAPI.Models
{
    public class Register
    {
        [Required, MaxLength(30)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required, MaxLength(150)]
        public string FullName { get; set; }
    }
}
