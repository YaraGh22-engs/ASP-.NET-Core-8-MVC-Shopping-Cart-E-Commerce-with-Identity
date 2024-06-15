using Microsoft.AspNetCore.Mvc.Rendering;

namespace Shopping_Cart_2.Services
{
    public interface ICategoryService
    {
        IEnumerable<SelectListItem> GetSelectList();
    }
}
