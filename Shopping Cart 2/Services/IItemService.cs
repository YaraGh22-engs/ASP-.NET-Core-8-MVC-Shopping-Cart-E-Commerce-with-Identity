using Shopping_Cart_2.Models;
using Shopping_Cart_2.ViewModels;

namespace Shopping_Cart_2.Services
{
    public interface IItemService
    {
        IEnumerable<Item> GetAll();
        IEnumerable<Item> GetItemsByUserId();
        Item? GetById(int id);
        Task Create (CreateItemVM vmItem , Stock st);
        Task<Item?> Update (EditItemVM vmItem);
        bool Delete (int id);
        
    }
}
