using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopRazor.Models.Db;

namespace OnlineShopRazor.Pages.Admin.Banners;

[Authorize(Roles = "admin")]
public class DetailsModel : PageModel
{
    private readonly OnlineDataBaseContext _context;
    public DetailsModel(OnlineDataBaseContext context)
    {
        _context = context;
    }

    public Banner Banner { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var banner = await _context.Banners.FirstOrDefaultAsync(m => m.Id == id);
        if (banner is null)
        {
            return NotFound();
        }
        else
        {
            Banner = banner;
        }

        return Page();
    }
}
