using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Pharmacy_v2.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy_v2.Models
{
    public class Category
    {
        public int Id { get; set; }

        [DisplayName("Category Name")]
        [Required(ErrorMessage = "You have to provide a valid name.")]
        [MinLength(2, ErrorMessage = "Name mustn't be less than 2 characters.")]
        [MaxLength(20, ErrorMessage = "Name mustn't exceed 20 characters.")]
        public string CategoryName { get; set; }


        [DisplayName("Yearly Budget")]
        [Required(ErrorMessage = "You have to provide a valid annual budget.")]
        [Range(0, 40000, ErrorMessage = "Annual budget must be between 0 EGP and 40000 EGP.")]
        public decimal AnnualBudget { get; set; }

        [DisplayName("Image")]
        [ValidateNever]
        public string ImagePath { get; set; }


        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }


        [ValidateNever]
        public List<Medicine> Medicines { get; set; }
    }
}
