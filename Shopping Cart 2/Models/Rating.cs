

namespace Shopping_Cart_2.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int RatingValue { get; set; }
        
        public DateTime ReviewDate { get; set; } = DateTime.Now;
        public string UserId { get; set; }  
        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public Item Item { get; set; } = default!;
    }
}
