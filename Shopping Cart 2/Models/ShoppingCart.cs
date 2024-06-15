using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping_Cart_2.Models
{
    [Table("ShoppingCart")]
    public class ShoppingCart
    {

        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }=string.Empty;
        public bool IsDeleted { get; set; } = false;
        public ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();
    }
}
