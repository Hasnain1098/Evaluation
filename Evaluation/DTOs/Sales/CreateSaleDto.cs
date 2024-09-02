using System.ComponentModel.DataAnnotations;

namespace Evaluation.DTOs.Sales
{
    public class CreateSaleDto
    {

        //public int? Id { get; set; } = null;
        // DTO property for Sale Date with validation
        [Required]
        public DateTime SaleDate { get; set; }

        // DTO property for Sale Amount with validation
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        // DTO property for Representative ID with validation
        [Required]
        public int RepresentativeId { get; set; }
    }
}

