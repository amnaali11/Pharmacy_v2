using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;

namespace Pharmacy_v2.Models
{
    public class Medicine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "You have to provide a valid name.")]
        [MinLength(2, ErrorMessage = "Name mustn't be less than 2 characters.")]
        [MaxLength(20, ErrorMessage = "Name mustn't exceed 20 characters.")]
        public string Name { get; set; }

        [DisplayName("Image")]
        [ValidateNever]
        public string? Picture { get; set; }

        [Required(ErrorMessage = "You have to provide a valid annual budget.")]
        [Range(0, 3000, ErrorMessage = "Annual budget must be between 0 EGP and 3000 EGP.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Production Date is required.")]
        [DataType(DataType.Date)]
        [ValidateProductionDate] // Custom validation attribute for production date
        public DateOnly ProductionDate { get; set; }

        [Required(ErrorMessage = "Expiry Date is required.")]
        [DataType(DataType.Date)]
        [ExpiryDateValidation] // Custom validation attribute for expiry date
        public DateOnly ExpiryDate { get; set; }

        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category category { get; set; }
    }
}
