using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopRazor.Models.Db;
using System.Security.Claims;

namespace OnlineShopRazor.Pages.User.Orders
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly OnlineShopRazor.Models.Db.OnlineDataBaseContext _context;

        public DetailsModel(OnlineShopRazor.Models.Db.OnlineDataBaseContext context)
        {
            _context = context;
        }

        public Order Order { get; set; } = default!;
        public List<OrderDetail> OrderDetails { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //-----------------
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var order = await _context.Orders.FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
            //-----------------
            OrderDetails = await _context.OrderDetails.Where(x => x.OrderId == id).ToListAsync();
            //-----------------
            if (order == null)
            {
                return NotFound();
            }
            else
            {
                Order = order;
            }
            //-----------------
            return Page();
        }
    }
}
