using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopRazor.Models.Db;

namespace OnlineShopRazor.Pages.Admin.Coupons;

[Authorize(Roles = "admin")]
public class DetailsModel : PageModel
{
    private readonly OnlineDataBaseContext _context;
    public DetailsModel(OnlineDataBaseContext context)
    {
        _context = context;
    }

    public Coupon Coupon { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var coupon = await _context.Coupons.FirstOrDefaultAsync(m => m.Id == id);
        if (coupon is null)
        {
            return NotFound();
        }
        else
        {
            Coupon = coupon;
        }

        return Page();
    }
}
