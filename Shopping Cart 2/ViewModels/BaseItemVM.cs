using Microsoft.AspNetCore.Mvc.Rendering;
using Shopping_Cart_2.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Shopping_Cart_2.ViewModels
{
    public class BaseItemVM
    {
        public string Name { get; set; } = string.Empty;
        [Required]
        public string? Description { get; set; } = string.Empty;
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than zero.")]
        public double Price { get; set; }  
        [Required]
        [Display(Name = "Quantity Available")]
        [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than zero.")]
        public int Quantity { get; set; }

        [Display(Name = "Category")]
        // to fill drop down list in view (category)
        public int CategoryId { get; set; } = 0; // asp-for
        public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>(); // asp-item


    }
}
