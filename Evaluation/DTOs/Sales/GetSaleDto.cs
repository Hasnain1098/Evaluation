using System.ComponentModel.DataAnnotations;

namespace Evaluation.DTOs.Sales
{
    public class GetSaleDto
    {

        [Required]
        public int Id{ get; set; }
        [Required]
        public DateTime SaleDate { get; set; }

        // DTO property for Sale Amount with validation
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }
    }
}
