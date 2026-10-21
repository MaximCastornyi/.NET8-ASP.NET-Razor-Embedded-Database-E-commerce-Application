using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopRazor.Models.Db;

namespace OnlineShopRazor.Pages.Admin.Coupons;

public class IndexModel : PageModel
{
    private readonly OnlineDataBaseContext _context;

    public IndexModel(OnlineDataBaseContext context)
    {
        _context = context;
    }

    public IList<Coupon> Coupon { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Coupon = await _context.Coupons.ToListAsync();
    }
}
