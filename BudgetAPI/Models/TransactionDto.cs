using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetAPI.Models
{
    public class TransactionDto
    {
        [Required(ErrorMessage ="Category is required!")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Payment Type is required!")]
        public PaymentType Type { get; set; }
        [Required(ErrorMessage = "Amount is required!")]
        public decimal Amount { get; set; }
        [MaxLength(500)]
        public string Desc { get; set; }
    }
}
