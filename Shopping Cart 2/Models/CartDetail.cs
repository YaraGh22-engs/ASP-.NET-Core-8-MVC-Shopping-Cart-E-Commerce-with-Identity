using System.ComponentModel.DataAnnotations;

namespace Shopping_Cart_2.Models
{
    public class CartDetail
    {
        public int Id { get; set; }
        [Required]
        public int ItemId { get; set; }
        public Item Item { get; set; } = default!;

        [Required]
        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; } = default!;
        public int Quantity { get; set; }
        [Required]
        public double UnitPrice { get; set; } 


    }
}
