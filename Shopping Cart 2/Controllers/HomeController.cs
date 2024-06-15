using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_Cart_2.Data;
using Shopping_Cart_2.Models;
using Shopping_Cart_2.Services;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shopping_Cart_2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IItemService _itemService;
         
        public HomeController(ILogger<HomeController> logger, IItemService itemService, ApplicationDbContext context )
        {
            _logger = logger;
            _itemService = itemService;
            _context = context;
             
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Products(string? seachName, string? categoryName )
        {
            var item = _itemService.GetAll();

            //Searching
            if (!string.IsNullOrEmpty(seachName))
            {
                item = item.Where(g => g.Name.ToLower().Contains(seachName.ToLower())
                    || g.Description.ToLower().Contains(seachName.ToLower())).ToList();
            }

            //Filtering
            else if (categoryName != null)
            {
                item = item.Where(g => g.Category.Name.ToLower() == categoryName.ToLower()).ToList();
            }
            ViewBag.seachName = seachName;
            ViewBag.categories = _context.categories.ToList();
              
            return View(item);
        
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
