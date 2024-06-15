using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping_Cart_2.Models
{
    [Table("Stock")]
    public class Stock
    {
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; } = 0;
         
        [ForeignKey(nameof(Item))]
        public int ItemId { get; set; }
        public Item? Item { get; set; } = default!;
    }
}
