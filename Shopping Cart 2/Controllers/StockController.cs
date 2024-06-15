namespace Shopping_Cart_2.Controllers
{
    [Authorize]
    public class StockController : Controller
    {
        private readonly IStockService _stockService;
        public StockController(IStockService stockService )
        {
            _stockService = stockService;
        }
        public async Task <IActionResult> Index()
        {
            var itemStock = await _stockService.GetStocks();
            return View(itemStock);
        }
        [HttpGet]
        public async Task<IActionResult> ManangeStock(int itemId)
        {
            var stock = await _stockService.GetStockByItemId(itemId);

            var dtoStock = new StockDTO
            {
                ItemId = itemId,
                Quantity = stock != null ? stock.Quantity : 0,
            };
            return View(dtoStock);
        }
        [HttpPost]
        public async Task<IActionResult> ManangeStock(StockDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);
            try
            {
                await _stockService.ManageStock(model);
                TempData["successMessage"] = "Stock is updated successfully.";
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Something went wrong!!";
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
