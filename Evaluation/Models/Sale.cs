using System.ComponentModel.DataAnnotations;

namespace Evaluation.Models
{
    public class Sale
    {
        public int Id { get; set; }

        [Required]
        public DateTime SaleDate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required]
        public int RepresentativeId { get; set; }
    }
}
