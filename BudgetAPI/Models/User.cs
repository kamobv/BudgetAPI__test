using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetAPI.Models
{
    public class User
    {
        public User()
        {
            Categories = new List<Category>();
        }
        public int Id { get; set; }
        [Required,StringLength(30),MaxLength(30),EmailAddress]
        public string Email { get; set; }
        [Required, StringLength(300),MaxLength(300)]
        public string Password { get; set; }
        [Required, StringLength(150), MaxLength(150)]
        public string FullName { get; set; }
        [Required]
        public DateTime CreateAt { get; set; }
        [StringLength(150), MaxLength(150)]
        public string Token { get; set; }
        public DateTime? TokenExpireDate { get; set; }
        public virtual List<Category> Categories { get; set; }
    }
}
