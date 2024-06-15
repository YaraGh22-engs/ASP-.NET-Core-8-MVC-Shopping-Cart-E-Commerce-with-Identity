using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping_Cart_2.Models
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        public Order Order { get; set; } = default!;
        [Required]
        public int ItemId { get; set; }
        public Item Item { get; set; } = default!;

        [Required]
        public int Quantity { get; set; }
        [Required]
        public double UnitPrice { get; set; }
    }
}
