using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetAPI.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public PaymentType Type { get; set; }
        [Required, Column(TypeName = "money")]
        public decimal Amoun { get; set; }
        [StringLength(500), MaxLength(500)]
        public string Desc { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DateTime CraeteAt { get; set; }
        public virtual Category Category { get; set; }
    }

    public enum PaymentType
    {
        Expense,
        Income
    }

}
