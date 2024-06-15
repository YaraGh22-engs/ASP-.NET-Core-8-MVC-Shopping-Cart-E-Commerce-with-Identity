namespace Shopping_Cart_2.Services
{
    public interface IManageItemService
    {
        Task<IEnumerable<Item>> GetAllItems();
        Task ToggleApprovementStatus(int ItemId);
    }
}
