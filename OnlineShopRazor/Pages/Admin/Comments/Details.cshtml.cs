using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopRazor.Models.Db;

namespace OnlineShopRazor.Pages.Admin.Comments;

[Authorize(Roles = "admin")]
public class DetailsModel : PageModel
{
    private readonly OnlineShopRazor.Models.Db.OnlineDataBaseContext _context;

    public DetailsModel(OnlineShopRazor.Models.Db.OnlineDataBaseContext context)
    {
        _context = context;
    }

    public Comment Comment { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var comment = await _context.Comments.FirstOrDefaultAsync(m => m.Id == id);
        if (comment == null)
        {
            return NotFound();
        }
        else
        {
            Comment = comment;
        }
        return Page();
    }
}

