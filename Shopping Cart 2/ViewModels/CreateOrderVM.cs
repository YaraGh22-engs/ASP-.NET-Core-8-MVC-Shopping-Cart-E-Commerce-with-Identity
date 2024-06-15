using Microsoft.AspNetCore.Mvc.Rendering;

namespace Shopping_Cart_2.ViewModels
{
    public class CreateOrderVM
    {
        public int Id { get; set; }
        
        
        public int Quantity { get; set; }
        

        //this tow props for select list input then we can select many items in the order
        public List<int> SelectedItems { get; set; } = new List<int>(); // // asp-for// because we want to select many items in the order
        public IEnumerable<SelectListItem> Items { get; set; }=Enumerable.Empty<SelectListItem>(); // asp-item
    }
}
