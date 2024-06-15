using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopping_Cart_2.Models;
using Shopping_Cart_2.Services;

namespace Shopping_Cart_2.Controllers
{
    [Authorize]
    public class UserOrderController : Controller
    {
        private readonly IUserOrderService _userOrderService;
        public UserOrderController(IUserOrderService userOrderService)
        {
            _userOrderService = userOrderService;
        }
        public async Task<IActionResult> UserOrders()
        {
            var orders = await _userOrderService.UserOrders();
            return View(orders);
        }
         
        public async Task<IActionResult> GetDetail(int orderId)
        {
            var detail =await _userOrderService.GetOrderDetail(orderId);
            return View(detail);
        }
        
    }
}
