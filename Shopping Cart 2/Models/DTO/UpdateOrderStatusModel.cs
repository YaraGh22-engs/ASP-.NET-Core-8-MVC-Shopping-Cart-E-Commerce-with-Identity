namespace Shopping_Cart_2.Models.DTO
{
    public class UpdateOrderStatusModel
    {
        public int OrderId { get; set; }

        [Required]
        public int OrderStatusId { get; set; }

        public IEnumerable<SelectListItem>? OrderStatusList { get; set; } = new List<SelectListItem>();
    }
}
