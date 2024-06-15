using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shopping_Cart_2.Constants;
using Shopping_Cart_2.Data;
using Shopping_Cart_2.Models;
using Shopping_Cart_2.Services;

namespace Shopping_Cart_2.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class AdminOperationsController : Controller
    {
        private readonly IUserOrderService _userOrderService;
        private readonly IManageItemService _manageItemService; 
        public AdminOperationsController(IUserOrderService userOrderService, IManageItemService manageItemService )
        {
            _userOrderService = userOrderService;
            _manageItemService = manageItemService;
             
            
        }


        public async Task<IActionResult> AllOrders()
        {
            var orders = await _userOrderService.AllOrders();
            return View(orders);
        }

        
        [HttpGet]
        public async Task<IActionResult> UpdateOrderStatus(int orderId)
        {
            var order = await _userOrderService.GetOrderById(orderId);
            if (order == null)
            {
                throw new InvalidOperationException($"Order with id:{orderId} does not found.");
            }
            var orderStatusList = _userOrderService.GetSelectLists();
            var data = new UpdateOrderStatusModel
            {
                OrderId = orderId,
                OrderStatusId = order.OrderStatusId,
                OrderStatusList = orderStatusList
            };
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(UpdateOrderStatusModel data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    data.OrderStatusList =  _userOrderService.GetSelectLists();
                    return View(data);
                }
                await _userOrderService.ChangeOrderStatus(data);
                TempData["msg"] = "Updated successfully";
            }
            catch
            {   // catch exception here
                TempData["msg"] = "Something went wrong";
            }
            return RedirectToAction(nameof(UpdateOrderStatus), new { orderId = data.OrderId });
        }

        public async Task<IActionResult> TogglePaymentStatus(int orderId)
        {
            try
            {
                await _userOrderService.TogglePaymentStatus(orderId);
            }
            catch (Exception ex)
            {
                // log exception here
            }
            return RedirectToAction(nameof(AllOrders));
        }
        public IActionResult Dashboard()
        {
            return View();
        }

        public async Task <IActionResult> ToggleApprovementStatus(int ItemId)
        {
            try
            {
                await _manageItemService.ToggleApprovementStatus(ItemId);
            }
            catch (Exception ex)
            {
                // log exception here
            }
            return RedirectToAction(nameof(GetAllItems));
             
        }
        public async Task <IActionResult> GetAllItems()
        {
            var items = await _manageItemService.GetAllItems();
            return View(items);
        }
        

         

    }
}
