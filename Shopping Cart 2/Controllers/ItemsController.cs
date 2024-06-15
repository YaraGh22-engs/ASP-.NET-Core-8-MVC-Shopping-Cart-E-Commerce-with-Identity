using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_Cart_2.Services;
using Shopping_Cart_2.ViewModels;

namespace Shopping_Cart_2.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IItemService _itemService;
        private readonly IRatingService _ratingService;
        private readonly IUserService _userService;

        public ItemsController(ICategoryService categoryService, IItemService itemService, IRatingService ratingService, IUserService userService)
        {
            _categoryService = categoryService;
            _itemService = itemService;
            _ratingService = ratingService;
            _userService = userService;
        }
        [Authorize]
        public IActionResult Index()
        {
            var items = _itemService.GetItemsByUserId(); 
            return View(items);
        }
        public IActionResult Details(int itemId)
        {
            var item =_itemService.GetById(itemId);
            if (item is null) return NotFound();

            var usertRate = _ratingService.GetUserRate(_userService.GetUserId(), itemId);
            ViewBag.userRate = usertRate;
            var productRate = _ratingService.GetProductRate(itemId);
            ViewBag.productRate = productRate;

            return View(item);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateItemVM vm = new()
            {
                Categories = _categoryService.GetSelectList(),
            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateItemVM model , Stock stock)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _itemService.Create(model,stock);
            return RedirectToAction ("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // 1- get item by id //اساسي
            var item = _itemService.GetById(id);
            if (item is null) return NotFound();
            // 3- create vm object
            // تعريف غرض وسيط و اسناد الاساسي للوسيط
            EditItemVM model = new()
            {
                Id = id,
                Name = item.Name,
                Description = item.Description,
                CategoryId = item.CategoryId,
                Price = item.Price,
                Categories = _categoryService.GetSelectList(),
                CurrentCover = item.Cover,
                Quantity = item.Stock.Quantity
                 

            };
            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditItemVM model )
        {
            // 5- تطبيق الخدمة على البارمتر الغرض الوسيط
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var item =await _itemService.Update(model);
            //if (item is null) return BadRequest();
            return RedirectToAction(nameof(Index));
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var isDeleted = _itemService.Delete(id);

            return isDeleted ? Ok() : BadRequest();
        }

        public IActionResult RateProduct(string ratingValue, int itemId)
        {
            var isRated = _ratingService.RateProduct(Int32.Parse(ratingValue), itemId, _userService.GetUserId());
            if (isRated < 0)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
