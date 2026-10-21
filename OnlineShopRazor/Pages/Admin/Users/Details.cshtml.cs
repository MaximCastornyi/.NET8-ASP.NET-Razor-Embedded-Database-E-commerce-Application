using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopRazor.Models.Db;

namespace OnlineShopRazor.Pages.Admin.Users;

[Authorize(Roles = "admin")]
public class DetailsModel : PageModel
{
    private readonly OnlineShopRazor.Models.Db.OnlineDataBaseContext _context;

    public DetailsModel(OnlineShopRazor.Models.Db.OnlineDataBaseContext context)
    {
        _context = context;
    }

    public OnlineShopRazor.Models.Db.User User { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
        if (user == null)
        {
            return NotFound();
        }
        else
        {
            User = user;
        }
        return Page();
    }
}