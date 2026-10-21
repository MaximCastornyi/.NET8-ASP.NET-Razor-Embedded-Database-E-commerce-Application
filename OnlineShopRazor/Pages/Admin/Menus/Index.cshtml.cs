using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopRazor.Models.Db;

namespace OnlineShopRazor.Pages.Admin.Menus
{
    [Authorize(Roles = "admin")]
    public class IndexModel : PageModel
    {
        private readonly OnlineShopRazor.Models.Db.OnlineDataBaseContext _context;

        public IndexModel(OnlineShopRazor.Models.Db.OnlineDataBaseContext context)
        {
            _context = context;
        }

        public IList<Menu> Menu { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Menu = await _context.Menus.ToListAsync();
        }
    }
}
