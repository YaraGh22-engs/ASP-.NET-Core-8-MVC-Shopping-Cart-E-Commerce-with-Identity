namespace Shopping_Cart_2.Models.DTO
{
    public class StockDTO
    {
        public int ItemId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non-negative value.")]
        public int Quantity { get; set; } = 0;
    }
}
