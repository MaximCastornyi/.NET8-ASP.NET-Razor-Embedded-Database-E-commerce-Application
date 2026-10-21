using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopRazor.Models.Db;

namespace OnlineShopRazor.Pages.Admin.Orders;

public class IndexModel : PageModel
{
    private readonly OnlineDataBaseContext _context;

    public IndexModel(OnlineDataBaseContext context)
    {
        _context = context;
    }

    public IList<Order> Order { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Order = await _context.Orders.ToListAsync();
    }
}
