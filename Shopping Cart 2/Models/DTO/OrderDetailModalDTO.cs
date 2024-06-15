namespace Shopping_Cart_2.Models.DTO
{
    public class OrderDetailModalDTO
    {
        [Key]
        public string DivId { get; set; } = string.Empty;
        public IEnumerable<OrderDetail> OrderDetail { get; set; }
    }
}
