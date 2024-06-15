using System.ComponentModel.DataAnnotations;

namespace Shopping_Cart_2.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;

        //for 1 -> M  category -> items
        public ICollection<Item?> Items { get; set; }=new List<Item>();

    }
}
