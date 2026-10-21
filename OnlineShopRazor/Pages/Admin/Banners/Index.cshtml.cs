using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopRazor.Models.Db;

namespace OnlineShopRazor.Pages.Admin.Banners;

public class IndexModel : PageModel
{
    private readonly OnlineDataBaseContext _context;

    public IndexModel(OnlineDataBaseContext context)
    {
        _context = context;
    }

    public IList<Banner> Banner { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Banner = await _context.Banners.ToListAsync();
    }
}
