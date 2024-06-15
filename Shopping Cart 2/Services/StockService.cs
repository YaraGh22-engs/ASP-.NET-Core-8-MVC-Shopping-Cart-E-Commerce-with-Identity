using Microsoft.EntityFrameworkCore;
using Shopping_Cart_2.Data;

namespace Shopping_Cart_2.Services
{
    public class StockService : IStockService
    {
        private readonly ApplicationDbContext _db;
        private readonly IUserService _userService;
        public StockService(ApplicationDbContext db, IUserService userService)
        {
            _db = db;
            _userService = userService;
        }

        public async Task<Stock?> GetStockByItemId(int itemId) 
        {
            var st=await _db.Stocks.Include(x=>x.Item)
                                    .FirstOrDefaultAsync(s => s.ItemId == itemId);
            return st;
                }

        public async Task<IEnumerable<StockDisplayModel>> GetStocks( )
        {

            var userId = _userService.GetUserId();
            if (userId == null)
                throw new UnauthorizedAccessException("user is not logged-in");
            //لانه list
            // اريد تحويلها 
            var itemsWithStock =await _db.items
                                         .Include(x=>x.Stock)
                                         .Where(x => x.UserId == userId)
                                         .Select(i => new StockDisplayModel
                                         { 
                                             ItemId = i.Id, 
                                             Quantity = i.Stock != null ? i.Stock.Quantity : 0,
                                             ItemName = i.Name
                                         }).ToListAsync();
             
            return itemsWithStock;
        }

        public async Task ManageStock(StockDTO stModel)
        {
            // if there is no stock for given book id, then add new record
            // if there is already stock for given book id, update stock's quantity
            var existingStock = await GetStockByItemId(stModel.ItemId);
            if (existingStock is null)
            {
                var stock = new Stock { ItemId = stModel.ItemId, Quantity = stModel.Quantity };
                await _db.Stocks.AddAsync(stock);
            }
            else
            {
                existingStock.Quantity = stModel.Quantity;
            }
            await _db.SaveChangesAsync();
        }
    }
}
