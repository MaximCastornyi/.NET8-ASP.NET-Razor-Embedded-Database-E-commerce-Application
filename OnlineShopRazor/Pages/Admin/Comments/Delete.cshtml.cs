using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopRazor.Models.Db;

namespace OnlineShopRazor.Pages.Admin.Comments;

[Authorize(Roles = "admin")]
public class DeleteModel : PageModel
{
    private readonly OnlineShopRazor.Models.Db.OnlineDataBaseContext _context;

    public DeleteModel(OnlineShopRazor.Models.Db.OnlineDataBaseContext context)
    {
        _context = context;
    }

    [BindProperty]
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

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var comment = await _context.Comments.FindAsync(id);
        if (comment != null)
        {
            Comment = comment;
            _context.Comments.Remove(Comment);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}

