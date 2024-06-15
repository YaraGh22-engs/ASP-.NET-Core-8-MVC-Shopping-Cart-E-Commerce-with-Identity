using Hangfire.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shopping_Cart_2.Data;
using Shopping_Cart_2.Models;

namespace Shopping_Cart_2.Services
{
    public class UserOrderService : IUserOrderService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserOrderService(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User; //currently authenticated user
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
        // get all orders for one user 
        public async Task<IEnumerable<Order>> UserOrders( )
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User is not logged-in");
            var orders =await _db.Orders
                           .Include(x => x.OrderStatus)
                           .Include(x => x.OrderDetail)
                           .ThenInclude(x => x.Item)
                           .ThenInclude(x => x.Category)
                           .Where(a => a.UserId == userId)
                           .ToListAsync(); 
            return orders;
        }
        public async Task<IEnumerable<Order>> AllOrders()
        {
            var orders = await _db.Orders
                          .Include(x => x.OrderStatus)
                          .Include(x => x.OrderDetail)
                          .ThenInclude(x => x.Item)
                          .ThenInclude(x => x.Category) 
                          .ToListAsync();
            return orders;
        }
        public async Task<Order?> GetOrderById(int id)
        {
            return await _db.Orders.SingleOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<Order> GetOrderDetail(int orderId)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User is not logged-in");
            var order = await _db.Orders
                           .Include(x => x.OrderStatus)
                           .Include(x => x.OrderDetail)
                           .ThenInclude(x => x.Item)
                           .ThenInclude(x => x.Category)
                           .Where(a => a.UserId == userId)
                           .SingleOrDefaultAsync(x => x.Id == orderId);
            return order;
        }
         
        public async Task ChangeOrderStatus(UpdateOrderStatusModel data)
        {
            var order = await _db.Orders.FindAsync(data.OrderId);
            if (order == null) throw new InvalidOperationException($"order within id:{data.OrderId} does not found");
            order.OrderStatusId=data.OrderStatusId;
            await _db.SaveChangesAsync();
        }
        public async Task TogglePaymentStatus(int orderId)
        {
            var order = await _db.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new InvalidOperationException($"order within id:{orderId} does not found");
            }
            order.IsPaid = !order.IsPaid;
            await _db.SaveChangesAsync();
        }

        public IEnumerable<SelectListItem> GetSelectLists()
        {

            return _db.orderStatuses.Select(os => new SelectListItem
            {
                Value = os.Id.ToString(),
                Text = os.StatusName
            }).ToList();
        }
    }
}
