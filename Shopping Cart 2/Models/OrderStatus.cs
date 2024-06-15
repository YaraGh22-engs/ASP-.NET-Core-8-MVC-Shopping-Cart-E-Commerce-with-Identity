using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping_Cart_2.Models
{
    [Table("OrderStatus")]
    public class OrderStatus
    {
        [Key]
         public int Id { get; set; }

        public int StatusId { get; set; }

        public string? StatusName { get; set; } = null;
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
