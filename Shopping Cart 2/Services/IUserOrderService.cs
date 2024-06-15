using Shopping_Cart_2.Models;
using System.Collections.Generic;

namespace Shopping_Cart_2.Services
{
    public interface IUserOrderService
    {
        
        Task<IEnumerable<Order>> UserOrders( );
        Task<Order?> GetOrderById(int id);
        Task<Order> GetOrderDetail(int orderId);
        //Task<IEnumerable<OrderStatus>> GetOrderStatuses();
        Task ChangeOrderStatus(UpdateOrderStatusModel data);
        Task TogglePaymentStatus(int orderId);
         IEnumerable<SelectListItem> GetSelectLists();
        Task<IEnumerable<Order>> AllOrders();
    }
}