using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required, StringLength(300), MaxLength(300)]
        public string Name { get; set; }
        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
