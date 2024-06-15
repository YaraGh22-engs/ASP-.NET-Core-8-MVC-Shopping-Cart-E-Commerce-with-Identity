namespace Shopping_Cart_2.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        public async Task<IActionResult> AddItem(int itemId, int qty = 1, int redirect = 0)
        {
            var cartCount = await _cartService.AddItem(itemId, qty);
            if (redirect == 0)
                return Ok(cartCount);
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> RemoveItem(int itemId)
        {
            var cartCount = await _cartService.RemoveItem(itemId); 
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartService.GetUserCart();
            return View(cart);
        }
        // for script in _layout.cshtml // to get total number of items in this cart // then pass it to other place in html
        public async Task<IActionResult> GetTotalItemInCart()
        {
            int cartItem = await _cartService.GetCartItemCount();
            return Ok(cartItem);
        }
        public IActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            bool isCheckedOut = await _cartService.DoCheckout(model);
            if (!isCheckedOut)
                return RedirectToAction(nameof(OrderFailure));
            return RedirectToAction(nameof(OrderSuccess));
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }

        public IActionResult OrderFailure()
        {
            return View();
        }

    }
}
