
using Shopping_Cart_2.Models;

namespace Shopping_Cart_2.Services
{
    public class ManageItemService : IManageItemService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IItemService _itemService;
        public ManageItemService(ApplicationDbContext context, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor, IItemService itemService)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _itemService = itemService;
        }

        public async Task< IEnumerable<Item>> GetAllItems()
        {
            var Item =await _context.items.Include(x => x.Category)
                                     .Include(x => x.Stock)
                                     .AsNoTracking()
                                     .ToListAsync();

            return Item;
        }
        public async Task ToggleApprovementStatus(int ItemId)
        {
            var item = await _context.items.FindAsync( ItemId);
            if (item == null)
            {
                throw new InvalidOperationException($"order within id:{ItemId} does not found");
            }
            item.IsApproved = !item.IsApproved;
            await _context.SaveChangesAsync();
        }
    }
}
