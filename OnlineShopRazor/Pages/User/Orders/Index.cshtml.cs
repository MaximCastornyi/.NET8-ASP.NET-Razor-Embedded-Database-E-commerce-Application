using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopRazor.Models.Db;
using System.Security.Claims;

namespace OnlineShopRazor.Pages.User.Orders
{

    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly OnlineShopRazor.Models.Db.OnlineDataBaseContext _context;

        public IndexModel(OnlineShopRazor.Models.Db.OnlineDataBaseContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get; set; } = default!;

        public async Task OnGetAsync()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Order = await _context.Orders.Where(x => x.UserId == userId).OrderByDescending(x => x.Id).ToListAsync();
        }
    }
}
