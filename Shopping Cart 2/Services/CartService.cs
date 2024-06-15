


namespace Shopping_Cart_2.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor,
            UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }



        //retrieves the user ID associated with the currently authenticated user
        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User; //currently authenticated user
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
        // للوصول الى كارت يوزر معين
        public async Task<ShoppingCart> GetCart(string userId)
        {

            var cart = await _db.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }
        public async Task<int> AddItem(int itmId, int qty)
        {
            //userId => ShCart => cartDItem
            string userId = GetUserId();
            // begin Transaction
            using var transaction = _db.Database.BeginTransaction();

            try
            {
                // الحصول على 1 id المستخدم  

                if (string.IsNullOrEmpty(userId))
                {
                    throw new UnauthorizedAccessException("user is not logged-in");
                }

                //2- جلب السلة الخاصة بالمستخدم
                var ShCart = await GetCart(userId);
                if (ShCart is null)
                {
                    ShCart = new ShoppingCart
                    {
                        UserId = userId
                    };
                    await _db.ShoppingCarts.AddAsync(ShCart);
                }
                await _db.SaveChangesAsync();

                //-3-  جبلي تفااااااصيل السلة يلي
                //الايدي تبعها يساوي ايدي سلة المستخدم الحالي
                //و يلي ايدي عنصرها يساوي الايدي الممرر بالضغط على الزر
                // cart detail section
                var cartDItem = await _db.CartDetails
                                  .FirstOrDefaultAsync(a => a.ShoppingCartId == ShCart.Id && a.ItemId == itmId);
                if (cartDItem is not null)
                {
                    cartDItem.Quantity += qty;
                    await _db.SaveChangesAsync();
                }
                else
                {
                    var item = await _db.items.FindAsync(itmId);

                    cartDItem = new CartDetail
                    {
                        ItemId = itmId,
                        ShoppingCartId = ShCart.Id,
                        Quantity = qty,
                        UnitPrice = item.Price
                    };
                    await _db.CartDetails.AddAsync(cartDItem);
                }
                await _db.SaveChangesAsync();
                transaction.Commit();

            }
            catch (Exception ex) { }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;

        }
        public async Task<int> RemoveItem(int itmId)
        {

            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("user is not logged-in");
                var ShCart = await GetCart(userId);
                if (ShCart is null)
                    throw new InvalidOperationException("Invalid cart");
                // cart detail section
                var cartDItem = _db.CartDetails
                                  .FirstOrDefault(a => a.ShoppingCartId == ShCart.Id && a.ItemId == itmId);
                if (cartDItem is null)
                    throw new InvalidOperationException("Not items in cart");
                else if (cartDItem.Quantity == 1)
                    _db.CartDetails.Remove(cartDItem);
                else
                    cartDItem.Quantity = cartDItem.Quantity - 1;
                _db.SaveChanges();

            }
            catch (Exception ex) { }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }
        public async Task<ShoppingCart> GetUserCart()
        { // للحصول على كل عناصر الكارت الواحد مع ارتباطاتها في الجداول المرتبطة بها
            var userId = GetUserId();
            if (userId == null)
                throw new InvalidOperationException("Invalid userid");
            var shoppingCart = await _db.ShoppingCarts
                                  .Include(a => a.CartDetails)
                                  .ThenInclude(a => a.Item)
                                  .ThenInclude(a=>a.Stock) // this for stock view 
                                  .Include(b => b.CartDetails)
                                  .ThenInclude(b => b.Item)
                                  .ThenInclude(b => b.Category)
                                  .Where(a => a.UserId == userId)
                                  .FirstOrDefaultAsync();
            return shoppingCart;
        }
        public async Task<int> GetCartItemCount(string userId = "")
        { // لمعرفة عدد العناصر في كل كارت
            if (string.IsNullOrEmpty(userId)) // updated line
            {
                userId = GetUserId();
            } 
            var sh = await _db.ShoppingCarts.Include(x=>x.CartDetails).SingleOrDefaultAsync(x => x.UserId == userId);
            if (sh == null)
            {
                return 0;
            }
            var data = sh.CartDetails.Sum(x => x.Quantity);

            //var totalQuantity = await (from cart in _db.ShoppingCarts
            //                           join cartDetail in _db.CartDetails
            //                           on cart.Id equals cartDetail.ShoppingCartId
            //                           where cart.UserId == userId
            //                           select cartDetail.Quantity).SumAsync();
            return data;
        }
        public async Task<bool> DoCheckout(CheckoutModel model)
        {
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                // logic
                // move data from cartDetail to order and order detail then we will remove cart detail
                //1- get user id
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("User is not logged-in");

                //2- get shopping cart of user id
                var ShCart = await GetCart(userId);
                if (ShCart is null)
                    throw new InvalidOperationException("Invalid cart");

                //3- get all carts detail of the shopping cart
                var cartDetail =  _db.CartDetails
                                    .Where(a => a.ShoppingCartId == ShCart.Id).ToList();
                if (cartDetail.Count == 0)
                    throw new InvalidOperationException("Cart is empty");
                //check pending state
                var pendingRecord = _db.orderStatuses.FirstOrDefault(s => s.StatusName == "Pending");
                if (pendingRecord is null)
                    throw new InvalidOperationException("Order status does not have Pending status");


                //4- create new order and add it to dataset
                Order order = new Order
                {
                    UserId = userId,
                    CreateDate = DateTime.UtcNow,
                    OrderStatusId = pendingRecord.Id, // pindding
                    Name = model.Name,
                    Email = model.Email,
                    MobileNumber = model.MobileNumber,
                    PaymentMethod = model.PaymentMethod,
                    Address = model.Address,
                    IsPaid = false

                };
                await _db.Orders.AddAsync(order);
                await _db.SaveChangesAsync();

                //5- create order detail for each cart detail
                foreach (var c in cartDetail)
                {
                    var orderDetail = new OrderDetail
                    {
                        ItemId = c.ItemId,
                        OrderId = order.Id,
                        Quantity = c.Quantity,
                        UnitPrice = c.UnitPrice
                    };
                    await _db.OrderDetails.AddAsync(orderDetail);

                    //Update the stock here
                    var stock = await _db.Stocks.FirstOrDefaultAsync(a => a.ItemId == c.ItemId);
                    if(stock is null)
                    {
                        throw new InvalidOperationException("Stock is null");
                    }
                    if( c.Quantity > stock.Quantity)
                    {
                        throw new InvalidOperationException($"Only {stock.Quantity} items(s) are available in the stock");
                    }
                    // decrease the number of quantity from the stock table
                    stock.Quantity -= c.Quantity;
                } 

                //6- removing the cartdetails
                _db.CartDetails.RemoveRange(cartDetail);
                _db.SaveChanges();
                transaction.Commit();
                return true;

            }

            catch (Exception ex)
            {

                return false;
            }
        }

    }
}
